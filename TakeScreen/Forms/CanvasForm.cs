using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;
using TakeScreen.Forms;
using System.Runtime.InteropServices;
using System.Reflection;

namespace TakeScreen
{
    public partial class CanvasForm : Form
    {
        #region Variables

        int pX = -1;
        int pY = -1;

        bool drawBorders = true;
        bool drawHandles = true;
        bool mouseDown = false;

        float currBorderWidth = 2f;

        GrabHandle focusedHandle = GrabHandle.None;
        DrawingTool currDrawingTool = DrawingTool.Arrow;
        RectangleState rectState = RectangleState.None;
        
        //Bitmap drawing;
        Rectangle selectionRect;
        Rectangle opacityRect;

        // Circle and rectangle variables
        Dictionary<GrabHandle, RectangleF> rectangles;

        // Assets - replaced basic shapes 
        List<Asset> assets;
        Asset currAsset;

        Font dimensions;

        SolidBrush brush;
        Image screenshot;
        Dictionary<Screen, Image> screenshots;

        private RegisterHotKeyClass _regisKey;

        /// <summary>
        /// Just an instance
        /// </summary>
        DrawingToolBox toolbox;
        ManipulateToolBox manipToolbox;

        // Graphics path
        GraphicsPath path;


        #endregion

        /// <summary>
        /// For per monitor DPI scaling 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [DllImport("shcore.dll")]
        static extern int SetProcessDpiAwareness(_Process_DPI_Awareness value);

        enum _Process_DPI_Awareness
        {
            Process_DPI_Unaware = 0,
            Process_System_DPI_Aware = 1,
            Process_Per_Monitor_DPI_Aware = 2
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public CanvasForm()
        {
            SetProcessDpiAwareness(_Process_DPI_Awareness.Process_Per_Monitor_DPI_Aware);

            path = new GraphicsPath();

            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            UpdateStyles();

            InitializeComponent();

            typeof(Panel).InvokeMember("DoubleBuffered", BindingFlags.SetProperty
            | BindingFlags.Instance | BindingFlags.NonPublic, null,
            pnlDraw, new object[] { true });

            typeof(Panel).InvokeMember("DoubleBuffered", BindingFlags.SetProperty
            | BindingFlags.Instance | BindingFlags.NonPublic, null,
            pnlOpacity, new object[] { true });

            typeof(Panel).InvokeMember("DoubleBuffered", BindingFlags.SetProperty
            | BindingFlags.Instance | BindingFlags.NonPublic, null,
            pnlPicture, new object[] { true });

            toolbox = new DrawingToolBox();
            manipToolbox = new ManipulateToolBox();

            assets = new List<Asset>();

            opacityRect = new Rectangle(0, 0, SystemInformation.VirtualScreen.Width, SystemInformation.VirtualScreen.Height);
            brush = new SolidBrush(Color.FromArgb(150, 80, 80, 80));
            focusedHandle = GrabHandle.None;

            rectangles = new Dictionary<GrabHandle, RectangleF>();
            foreach (GrabHandle handle in Enum.GetValues(typeof(GrabHandle)))
            {
                rectangles[handle] = new RectangleF(-5, -5, 7, 7);
                path.AddRectangle(rectangles[handle]);
            }

            dimensions = new Font(Font.FontFamily, 9, FontStyle.Regular | FontStyle.Bold);
            screenshots = new Dictionary<Screen, Image>();

            _regisKey = new RegisterHotKeyClass();

            RegisterEvents();
        }

        /// <summary>
        /// Registers events for all forms
        /// </summary>
        private void RegisterEvents()
        {
            Load += CanvasForm_Load;
            //MouseDown += Canvas_MouseDown;
            //MouseUp += Canvas_MouseUp;
            //MouseMove += Canvas_MouseMove;
            MouseWheel += CanvasForm_MouseWheel;

            toolbox.ToolChanged += Toolbox_ToolChanged;
            toolbox.btnCancel.Click += BtnCancel_Click;

            manipToolbox.btnSave.MouseClick += SaveScreen_MouseClick;
            manipToolbox.btnUndo.MouseClick += BtnUndo_MouseClick;

            pnlPicture.Paint += PnlPicture_Paint;
            pnlOpacity.Paint += PnlOpacity_Paint;
            pnlDraw.Paint += PnlDraw_Paint;
            pnlDraw.MouseDown += Canvas_MouseDown;
            pnlDraw.MouseMove += Canvas_MouseMove;
            pnlDraw.MouseUp += Canvas_MouseUp;
        }

       

        private void PnlDraw_Paint(object sender, PaintEventArgs e)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
            //e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            if (selectionRect.Width != 0 || selectionRect.Height != 0)
            {
                //path.AddRectangle(Helper.WriteText(e.Graphics,
                //    string.Format("{0} x {1}", selectionRect.Width, selectionRect.Height), Rectangle.Round(selectionRect)));

                // Draw grabhandles around the rectangle
                if (drawHandles)
                {
                    foreach (GrabHandle handle in Enum.GetValues(typeof(GrabHandle)))
                    {
                        if (handle == GrabHandle.None) continue;

                        var rect = rectangles[handle];
                        rect.X = Helper.GetRectangleBounds(selectionRect, handle, 6f).X;
                        rect.Y = Helper.GetRectangleBounds(selectionRect, handle, 6f).Y;

                        ControlPaint.DrawGrabHandle(e.Graphics, Rectangle.Round(rect), true, true);
                        e.Graphics.ExcludeClip(Rectangle.Round(rect));

                        rectangles[handle] = rect;
                    }
                }

                // Draw border around the rectangle            
                if (drawBorders)
                {
                    ControlPaint.DrawBorder(e.Graphics, Rectangle.Round(selectionRect), Color.White, ButtonBorderStyle.Solid);
                    ControlPaint.DrawBorder(e.Graphics, Rectangle.Round(selectionRect), Color.Black, ButtonBorderStyle.Dashed);
                }
            }
            //else
            //    path.AddRectangle(Helper.WriteText(e.Graphics, "Select area", PointToClient(new Point(Cursor.Position.X, Cursor.Position.Y)), 20));

            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            // Assets now support undo
            foreach (Asset asset in assets)
            {
                if (asset.Shape == Shape.ArrowLine)
                {
                    using (var pen = new Pen(asset.Color, asset.BorderWidth) { StartCap = LineCap.Round })
                    using (AdjustableArrowCap bigArrow = new AdjustableArrowCap(5, 5))
                    {
                        pen.CustomEndCap = bigArrow;
                        e.Graphics.DrawLine(pen, asset.LineStart, asset.LineEnd);
                    }
                }
                else if (asset.Shape == Shape.Line)
                    using (var pen = new Pen(asset.Color, asset.BorderWidth) { StartCap = LineCap.Round, EndCap = LineCap.Round })
                        e.Graphics.DrawLine(pen, asset.LineStart, asset.LineEnd);
                else if (asset.Shape == Shape.Circle)
                    using (var pen = new Pen(asset.Color, asset.BorderWidth) { StartCap = LineCap.Round, EndCap = LineCap.Round })
                        e.Graphics.DrawEllipse(pen, asset.Rectangle);
                else if (asset.Shape == Shape.Rectangle)
                    using (var pen = new Pen(asset.Color, asset.BorderWidth) { StartCap = LineCap.Flat, EndCap = LineCap.Flat })
                        e.Graphics.DrawRectangle(pen, asset.Rectangle.X, asset.Rectangle.Y, asset.Rectangle.Width, asset.Rectangle.Height);
                else if (asset.Shape == Shape.Pen)
                    if (asset.Points.Count > 1)
                        using (var pen = new Pen(asset.Color, asset.BorderWidth) { StartCap = LineCap.Round, EndCap = LineCap.Round })
                            e.Graphics.DrawCurve(pen, asset.Points.ToArray());
            }

            // Draw current assets on mouse down
            using (var pen = new Pen(toolbox.SelectedColor, currBorderWidth) { StartCap = LineCap.Round, EndCap = LineCap.Round })
            {
                if (mouseDown)
                {
                    if (currDrawingTool == DrawingTool.Rectangle)
                        e.Graphics.DrawRectangle(pen, currAsset.Rectangle.X, currAsset.Rectangle.Y, currAsset.Rectangle.Width, currAsset.Rectangle.Height);
                    else if (currDrawingTool == DrawingTool.Circle)
                        e.Graphics.DrawEllipse(pen, currAsset.Rectangle);
                    else if (currDrawingTool == DrawingTool.Pen)
                    {
                        if (currAsset.Points.Count > 1)
                            e.Graphics.DrawCurve(pen, currAsset.Points.ToArray());
                    }
                    else if (currDrawingTool == DrawingTool.Line)
                        e.Graphics.DrawLine(pen, currAsset.LineStart, currAsset.LineEnd);
                    else if (currDrawingTool == DrawingTool.ArrowLine)
                    {
                        using (AdjustableArrowCap bigArrow = new AdjustableArrowCap(5, 5))
                        {
                            pen.CustomEndCap = bigArrow;
                            e.Graphics.DrawLine(pen, currAsset.LineStart, currAsset.LineEnd);
                        }
                    }
                }
            }

            if (currDrawingTool == DrawingTool.Text)
            {

            }

            // Draw tip of the Mouse
            if (currDrawingTool != DrawingTool.Arrow)
                e.Graphics.DrawEllipse(Pens.Black,
                    new RectangleF(PointToClient(Point.Round(new PointF(Cursor.Position.X - currBorderWidth / 2, Cursor.Position.Y - currBorderWidth / 2))),
                    new SizeF(currBorderWidth, currBorderWidth)));
            
            path.AddRectangle(new Rectangle(new Point(50, 50), new Size(100, 100)));

            watch.Stop();
            Helper.WriteText(e.Graphics, watch.Elapsed.TotalMilliseconds.ToString(), PointToClient(new Point(50, 50)), 0);
        }

        private void PnlOpacity_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.HighSpeed;
            e.Graphics.CompositingQuality = CompositingQuality.HighSpeed;
            //e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;   

            e.Graphics.ExcludeClip(selectionRect);

            //Alter screenshot opacity
            e.Graphics.FillRectangle(brush, opacityRect);
        }

        private void PnlPicture_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.HighSpeed;
            e.Graphics.CompositingQuality = CompositingQuality.HighSpeed;
            //e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;

            //Draw screenshot on the form
            foreach (Screen screen in screenshots.Keys)
                e.Graphics.DrawImage(screenshots[screen], RectangleToClient(screen.Bounds));
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            Hide();
            WindowState = FormWindowState.Minimized;
            RefreshCanvas();
        }

        /// <summary>
        /// Changes the draw width
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CanvasForm_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
                currBorderWidth = currBorderWidth < 10.5f ? currBorderWidth + 1 : currBorderWidth;
            else
                currBorderWidth = currBorderWidth > 2.5f ? currBorderWidth - 1 : currBorderWidth;

            pnlDraw.Invalidate(Rectangle.Round(path.GetBounds()));
        }

        /// <summary>
        /// Performs the asset undo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnUndo_MouseClick(object sender, MouseEventArgs e)
        {
            if (assets.Count > 0)
                assets.Remove(assets[assets.Count - 1]);

            pnlDraw.Refresh();
        }

        /// <summary>
        /// Event triggers when save clicked from inside toolbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveScreen_MouseClick(object sender, MouseEventArgs e)
        {
            using (Bitmap bmp = new Bitmap((int)selectionRect.Width, (int)selectionRect.Height))
            using (Graphics g = Graphics.FromImage(bmp as Image))
            {
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

                drawBorders = false;
                drawHandles = false;

                Refresh();
                g.CopyFromScreen(PointToScreen(Point.Ceiling(selectionRect.Location)), new Point(0, 0), Rectangle.Ceiling(selectionRect).Size);

                using (var sfd = new SaveFileDialog() { Filter = "PNG|*.png" })
                {
                    if (sfd.ShowDialog() == DialogResult.OK)
                        bmp.Save(sfd.FileName);
                }
            }

            SendKeys.Send("{ESC}");
            SendKeys.Flush();            

            RefreshCanvas();
        }

        /// <summary>
        /// Event triggers when toolbox tool changes selection
        /// </summary>
        /// <param name="currentTool"></param>
        private void Toolbox_ToolChanged(DrawingTool currentTool)
        {
            currDrawingTool = toolbox.CurrentDrawingTool;
        }

        /// <summary>
        /// Logic when selection draw starts
        /// drawing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Canvas_MouseDown(object sender, MouseEventArgs e)
        {
            drawBorders = true;
            drawHandles = true;
            mouseDown = true;

            switch (currDrawingTool)
            {
                case DrawingTool.Arrow:
                    bool inPos = false;
                    foreach (GrabHandle handle in Enum.GetValues(typeof(GrabHandle)))
                    {
                        if (rectangles[handle].Contains(e.Location))
                        {
                            inPos = true;
                            focusedHandle = handle;
                            break;
                        }                        
                    }

                    if (inPos)
                        rectState = RectangleState.Resize;
                    else if (selectionRect.Contains(e.Location) && !inPos)
                        rectState = RectangleState.Move;
                    else
                    {
                        rectState = RectangleState.Draw;
                        toolbox.Visible = false;
                        manipToolbox.Visible = false;
                        path = new GraphicsPath();
                    }
                    break;
                case DrawingTool.Circle:
                    var circle = new Asset()
                    {
                        BorderWidth = currBorderWidth,
                        Color = toolbox.SelectedColor,
                        Shape = Shape.Circle,
                        Rectangle = new Rectangle(Cursor.Position.X, Cursor.Position.Y, 0, 0)
                    };
                    currAsset = circle;
                    break;
                case DrawingTool.ArrowLine:
                case DrawingTool.Line:
                    var line = new Asset()
                    {
                        BorderWidth = currBorderWidth,
                        Color = toolbox.SelectedColor,
                        Shape = currDrawingTool == DrawingTool.Line ? Shape.Line : Shape.ArrowLine,
                        LineStart = new Point(e.X, e.Y),
                        LineEnd = new Point(e.X, e.Y)
                    };
                    currAsset = line;
                    break;
                case DrawingTool.Pen:
                    var pen = new Asset()
                    {
                        BorderWidth = currBorderWidth,
                        Color = toolbox.SelectedColor,
                        Shape = Shape.Pen,
                        Points = new List<PointF>()
                    };

                    pen.Points.Add(e.Location);
                    currAsset = pen;
                    break;
                case DrawingTool.Rectangle:
                    var rectangle = new Asset()
                    {
                        BorderWidth = currBorderWidth,
                        Color = toolbox.SelectedColor,
                        Shape = Shape.Rectangle,
                        Rectangle = new RectangleF(Cursor.Position.X, Cursor.Position.Y, 0, 0)
                    };
                    currAsset = rectangle;
                    break;
                case DrawingTool.Text:
                    break;
            }

            pX = e.X;
            pY = e.Y;

            pnlDraw.Invalidate(Rectangle.Round(path.GetBounds()));
        }

        /// <summary>
        /// Actually contains the selection draw logic 
        /// for the rectangle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            //path.AddRectangle(selectionRect);

            if (selectionRect.Height > 0 && selectionRect.Width > 0 && selectionRect.Contains(e.Location) &&
            !rectangles[GrabHandle.TopLeft].Contains(e.Location) && !rectangles[GrabHandle.TopMid].Contains(e.Location) &&
            !rectangles[GrabHandle.TopRight].Contains(e.Location) && !rectangles[GrabHandle.BottomLeft].Contains(e.Location) &&
            !rectangles[GrabHandle.BottomMid].Contains(e.Location) && !rectangles[GrabHandle.BottomRight].Contains(e.Location) &&
            !rectangles[GrabHandle.MidLeft].Contains(e.Location) && !rectangles[GrabHandle.MidRight].Contains(e.Location) &&
            currDrawingTool == DrawingTool.Arrow)
                Cursor = Cursors.SizeAll;
            else if (rectangles[GrabHandle.TopLeft].Contains(e.Location))
                Cursor = Cursors.SizeNWSE;
            else if (rectangles[GrabHandle.TopMid].Contains(e.Location))
                Cursor = Cursors.SizeNS;
            else if (rectangles[GrabHandle.TopRight].Contains(e.Location))
                Cursor = Cursors.SizeNESW;
            else if (rectangles[GrabHandle.BottomLeft].Contains(e.Location))
                Cursor = Cursors.SizeNESW;
            else if (rectangles[GrabHandle.BottomMid].Contains(e.Location))
                Cursor = Cursors.SizeNS;
            else if (rectangles[GrabHandle.BottomRight].Contains(e.Location))
                Cursor = Cursors.SizeNWSE;
            else if (rectangles[GrabHandle.MidLeft].Contains(e.Location))
                Cursor = Cursors.SizeWE;
            else if (rectangles[GrabHandle.MidRight].Contains(e.Location))
                Cursor = Cursors.SizeWE;
            else if (currDrawingTool == DrawingTool.Pen)
                Cursor = DefaultCursor;
            else if (currDrawingTool == DrawingTool.Arrow)
                Cursor = DefaultCursor;
            else
                Cursor = DefaultCursor;

            if (rectState == RectangleState.Draw && currDrawingTool == DrawingTool.Arrow)
            {
                Controls.Remove(toolbox);
                Controls.Remove(manipToolbox);

                selectionRect.X = e.X > pX ? pX : e.X;
                selectionRect.Y = e.Y > pY ? pY : e.Y;

                selectionRect.Width = Math.Abs(e.X - pX);// e.X > pX ? e.X - pX : pX - e.X;
                selectionRect.Height = Math.Abs(e.Y - pY); // e.Y > pY ? e.Y - pY : pY - e.Y;

                toolbox.Location = GetToolBoxLocation();
                manipToolbox.Location = GetManipToolboxLoc();
            }
            else if (rectState == RectangleState.Move && currDrawingTool == DrawingTool.Arrow)
            {
                selectionRect.X += e.X - pX;
                selectionRect.Y += e.Y - pY;

                pX = e.X;
                pY = e.Y;

                toolbox.Location = GetToolBoxLocation();
                manipToolbox.Location = GetManipToolboxLoc();
            }
            else if ((currDrawingTool == DrawingTool.Rectangle || currDrawingTool == DrawingTool.Circle) &&
                e.Button == MouseButtons.Left)
            {
                currAsset.Rectangle = new Rectangle(e.X > pX ? pX : e.X,
                    e.Y > pY ? pY : e.Y, e.X > pX ? e.X - pX : pX - e.X,
                    e.Y > pY ? e.Y - pY : pY - e.Y);

                path.AddRectangle(currAsset.Rectangle);
            }
            else if (currDrawingTool == DrawingTool.Pen && e.Button == MouseButtons.Left)
            {
                currAsset.Points.Add(e.Location);
                path.AddCurve(currAsset.Points.ToArray());
            }
            else if ((currDrawingTool == DrawingTool.Line || currDrawingTool == DrawingTool.ArrowLine) &&
                e.Button == MouseButtons.Left)
            {
                currAsset.LineEnd = e.Location;
                path.AddLine(currAsset.LineStart, currAsset.LineEnd);
            }
            else if (rectState == RectangleState.Resize && focusedHandle != GrabHandle.None)
            {
                // Thats a tight code - do not mess with it

                int diff = Math.Abs(e.X - pX);

                switch (focusedHandle)
                {
                    case GrabHandle.TopLeft:
                        selectionRect.Width = pX > e.X ? selectionRect.Width + diff : selectionRect.Width - diff;
                        selectionRect.X = selectionRect.X + e.X - pX;
                        selectionRect.Height = selectionRect.Height + pY - e.Y;
                        selectionRect.Y = selectionRect.Y + e.Y - pY;
                        break;
                    case GrabHandle.TopMid:
                        selectionRect.Y = selectionRect.Y + e.Y - pY;
                        selectionRect.Height = selectionRect.Height + pY - e.Y;
                        break;
                    case GrabHandle.TopRight:
                        selectionRect.Width = selectionRect.Width + e.X - pX;
                        selectionRect.Height = selectionRect.Height - e.Y + pY;
                        selectionRect.Y = selectionRect.Y + e.Y - pY;
                        break;
                    case GrabHandle.BottomLeft:
                        selectionRect.Width = selectionRect.Width + pX - e.X;
                        selectionRect.X = selectionRect.X + e.X - pX;
                        selectionRect.Height = selectionRect.Height + e.Y - pY;
                        break;
                    case GrabHandle.BottomMid:
                        selectionRect.Height = selectionRect.Height + e.Y - pY;
                        break;
                    case GrabHandle.BottomRight:
                        selectionRect.Width = selectionRect.Width + e.X - pX;
                        selectionRect.Height = selectionRect.Height + e.Y - pY;
                        break;
                    case GrabHandle.MidLeft:
                        selectionRect.Width = (selectionRect.Width + pX - e.X) < 0 ? selectionRect.Width + e.X - pX : selectionRect.Width + pX - e.X;
                        selectionRect.X = selectionRect.X + e.X - pX;
                        break;
                    case GrabHandle.MidRight:
                        selectionRect.Width = selectionRect.Width + e.X - pX;
                        break;
                }

                pX = e.X;
                pY = e.Y;

                // Move toolbox with the rectangle
                toolbox.Location = GetToolBoxLocation();
                manipToolbox.Location = GetManipToolboxLoc();
            }

            foreach (GrabHandle handle in Enum.GetValues(typeof(GrabHandle)))
                path.AddRectangle(rectangles[handle]);

            pnlDraw.Invalidate(Rectangle.Inflate(Rectangle.Round(path.GetBounds()), 2,2));
        }

        /// <summary>
        /// Controls mouse up event handler after
        /// Selection rectangle has been drawn
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Canvas_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;

            if (currDrawingTool == DrawingTool.Rectangle)
            {
                assets.Add(currAsset);
                path.AddRectangle(currAsset.Rectangle);
            }
            else if (currDrawingTool == DrawingTool.Circle)
            {
                assets.Add(currAsset);
                path.AddRectangle(currAsset.Rectangle);
            }
            else if (currDrawingTool == DrawingTool.Pen)
            {
                if (currAsset.Points.Count == 1)
                    currAsset.Points.Add(e.Location);

                if (currAsset.Points.Count > 1)
                    assets.Add(currAsset);

                path.AddCurve(currAsset.Points.ToArray());

            }
            else if (currDrawingTool == DrawingTool.Line || currDrawingTool == DrawingTool.ArrowLine)
            {
                assets.Add(currAsset);
                path.AddLine(currAsset.LineStart, currAsset.LineEnd);
            }
            if (selectionRect.Height > 0 || selectionRect.Width > 0)
            {
                path.AddRectangle(selectionRect);

                toolbox.TopLevel = false;
                Controls.Add(toolbox);
                toolbox.Location = GetToolBoxLocation();
                toolbox.Show();
                toolbox.BringToFront();

                manipToolbox.TopLevel = false;
                Controls.Add(manipToolbox);
                manipToolbox.Location = GetManipToolboxLoc();
                manipToolbox.Show();
                manipToolbox.BringToFront();

                Focus();
            }

            rectState = RectangleState.None;
            focusedHandle = GrabHandle.None;
        }

        /// <summary>
        /// Gets the appropriate location for the toolbox to draw
        /// </summary>
        /// <returns></returns>
        private Point GetToolBoxLocation()
        {
            int left = selectionRect.Left - toolbox.Width < 0 ? 4 :
                (int)selectionRect.Left - toolbox.Width - 4;
            int top = toolbox.Bottom + 4 > SystemInformation.VirtualScreen.Height ? SystemInformation.VirtualScreen.Height - toolbox.Height - 4 :
                        (int)selectionRect.Top;

            return new Point(left, top);
        }

        /// <summary>
        /// Gets the appropriate location for the manip toobox
        /// </summary>
        /// <returns></returns>
        private Point GetManipToolboxLoc()
        {
            int left = (int)selectionRect.Left < 0 ? 4 : (int)selectionRect.Left;
            int top = selectionRect.Bottom + manipToolbox.Height + 4 > SystemInformation.VirtualScreen.Height ?
                SystemInformation.VirtualScreen.Height - manipToolbox.Height - 4 : (int)selectionRect.Bottom + 4;

            return new Point(left, top);
        }

        /// <summary>
        /// Reset all rectangles in canvas
        /// </summary>
        private void RefreshCanvas()
        {
            path = new GraphicsPath();

            toolbox.ResetDrawingTool();

            Controls.Remove(toolbox);
            toolbox.Visible = false;

            Controls.Remove(manipToolbox);
            manipToolbox.Visible = false;

            foreach (GrabHandle handle in Enum.GetValues(typeof(GrabHandle)))
                rectangles[handle] = new RectangleF(-5, -5, 7, 7);

            selectionRect = new Rectangle();            
            focusedHandle = GrabHandle.None;

            foreach (Image img in screenshots.Values)
                img.Dispose();

            screenshots.Clear();

            if (screenshot != null)
            {
                screenshot.Dispose();
                screenshot = null;
            }

            assets.Clear();

            GC.Collect();
        }

        /// <summary>
        /// Controls all events for pre and post
        /// loading screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CanvasForm_Load(object sender, EventArgs e)
        {
            _regisKey.Keys = Keys.PrintScreen;
            _regisKey.ModKey = 0;
            _regisKey.WindowHandle = this.Handle;
            _regisKey.HotKey += HotKeyPressed;

            try
            {
                _regisKey.StarHotKey();
            }
            catch (Exception ex)
            {
                notifyIcon1.ShowBalloonTip(10000, "Failed to register hotkey ...", ex.Message, ToolTipIcon.Error);
            }

            Hide();
        }

        /// <summary>
        /// Controls the behavior of hotkey pressed
        /// with the form
        /// </summary>
        private void HotKeyPressed()
        {
            //drawScreenShot = true;
            RefreshCanvas();

            foreach (Screen screen in Screen.AllScreens)
            {
                screenshot = new Bitmap(screen.Bounds.Width, screen.Bounds.Height, PixelFormat.Format32bppArgb);
                using (var screenstate = Graphics.FromImage(screenshot))
                    screenstate.CopyFromScreen(screen.Bounds.X, screen.Bounds.Y, 0, 0, screen.Bounds.Size);
                screenshots[screen] = screenshot;

            }

            Width = SystemInformation.VirtualScreen.Width;
            Height = SystemInformation.VirtualScreen.Height;

            Show();

            WindowState = FormWindowState.Normal;

            Top = SystemInformation.VirtualScreen.Top;
            Left = SystemInformation.VirtualScreen.Left;

#if !DEBUG //Enables the app to be topmost when in release mode
            TopMost = true;
#endif            

            GC.Collect();
        }

        /// <summary>
        /// Controls the escape key 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CanvasForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch ((char)(e.KeyChar))
            {
                case (char)Keys.Escape:
                    WindowState = FormWindowState.Minimized;
                    RefreshCanvas();
                    break;
                case (char)Keys.Up:
                    if (selectionRect.Width > 0 || selectionRect.Height > 0)
                        selectionRect.Y -= 1;
                    break;
            }
        }

        /// <summary>
        /// Release client area of current drawings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CanvasForm_Deactivate(object sender, EventArgs e)
        {
            _regisKey.HotKey += HotKeyPressed;
            TopMost = false;
        }

        /// <summary>
        /// Triggers event for form activation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CanvasForm_Activated(object sender, EventArgs e)
        {
            _regisKey.HotKey -= HotKeyPressed;
        }

        /// <summary>
        /// Closes app from notifyform
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Disposes from after closing 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CanvasForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            notifyIcon1.Dispose();
        }

        /// <summary>
        /// Takes screenshot
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void takeScreenshotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HotKeyPressed();
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                contextMenuStrip1.Show(Cursor.Position);
            else if (e.Button == MouseButtons.Left)
                HotKeyPressed();
        }
    }
}
