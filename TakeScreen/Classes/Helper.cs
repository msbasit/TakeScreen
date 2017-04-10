using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace TakeScreen
{
    // This will create temporary image file for the screenshots
    public static class Helper
    {
        /// <summary>
        /// This pen will be used throughout program
        /// No need to create a new instance of pen for 
        /// every drawing - memory optimization
        /// </summary>
        private static Pen pen;

        /// <summary>
        /// Standard pen with 2.5f size tip
        /// </summary>
        private static Pen standatdPen;

        /// <summary>
        /// Static constructor
        /// </summary>
        static Helper()
        {
            pen = new Pen(Color.White, 1f);
            pen.Alignment = PenAlignment.Outset;

            standatdPen = new Pen(Color.Red, 2.5f);
        }

        public static string SAVEPATH = null;

        /// <summary>
        /// Pencil configurations
        /// </summary>
        public static Pen Pencil
        {
            get
            {
                pen.DashStyle = DashStyle.Custom;
                pen.DashPattern = new float[] { 3f, 3f };
                //pen.EndCap = LineCap.NoAnchor;
                //pen.StartCap = LineCap.NoAnchor;
                pen.Alignment = PenAlignment.Center;
                pen.Color = Color.Black;
                return pen;
            }
        }

        /// <summary>
        /// Provides standard pen for drawings [To avoid disposition]
        /// </summary>
        public static Pen StdPen
        {
            get { return standatdPen; }
        }

        /// <summary>
        /// Gets a new Pen with desired parameters
        /// </summary>
        /// <param name="color"></param>
        /// <param name="width"></param>
        /// <param name="alignment"></param>
        /// <returns></returns>
        public static Pen GetPencil(Color color, float width, PenAlignment alignment)
        {
            return new Pen(color, width) { Alignment = alignment };
        }

        /// <summary>
        /// Changes image opacity for screenshot
        /// </summary>
        /// <param name="img"></param>
        /// <param name="opacityvalue"></param>
        /// <returns></returns>
        public static Bitmap ChangeOpacity(Image img, float opacityvalue)
        {
            Bitmap bmp = new Bitmap(img.Width, img.Height); // Determining Width and Height of Source Image
            Graphics graphics = Graphics.FromImage(bmp);
            ColorMatrix colormatrix = new ColorMatrix();
            colormatrix.Matrix33 = opacityvalue;
            ImageAttributes imgAttribute = new ImageAttributes();
            imgAttribute.SetColorMatrix(colormatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            graphics.DrawImage(img, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, imgAttribute);
            graphics.Dispose();   // Releasing all resource used by graphics 
            return bmp;
        }

        /// <summary>
        /// Draws text on the paint canvas via specified rect bounds
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="text"></param>
        /// <param name="rect"></param>
        public static Rectangle WriteText(Graphics graphics, string text, Rectangle rect)
        {
            using (var dimensions = new Font(new FontFamily("Segoe UI Semilight"), 9, FontStyle.Bold))
            {
                Point textDrawPt = new Point(rect.X < 0 ? 6 : rect.X + 6,
                        rect.Y < 24 ?
                        rect.Y < 0 ? 6 : rect.Y + 6 :
                        rect.Y - TextRenderer.MeasureText(text, dimensions).Height - 4);

                Size textDrawSz = TextRenderer.MeasureText(text, dimensions);

                //+Calculating area for writing selectable region dimensions 
                Rectangle rectTextArea = new Rectangle(textDrawPt, textDrawSz);
                Region rgTextArea = new Region(rectTextArea);

                graphics.FillRegion(Brushes.WhiteSmoke, rgTextArea);
                graphics.DrawRectangle(Pens.Black, rectTextArea);

                // Write dimensions on the screen
                TextRenderer.DrawText(graphics, text, dimensions, textDrawPt, Color.Black, Color.Transparent);

                //// Exclud selected region area from opaque paint
                //graphics.ExcludeClip(rgTextArea);

                return rectTextArea;
            }
        }

        /// <summary>
        /// Draws text on the paint canvas with specified point with specified offset
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="text"></param>
        /// <param name="location"></param>
        /// <param name="offset"></param>
        public static Rectangle WriteText(Graphics graphics, string text, Point location, int offset)
        {
            using (var dimensions = new Font(new FontFamily("Segoe UI Semilight"), 9, FontStyle.Bold))
            {
                Screen s = Screen.FromPoint(new Point(Cursor.Position.X, Cursor.Position.Y));

                Size textDrawSz = TextRenderer.MeasureText(graphics, text, dimensions);
                Point textDrawPt = new Point(location.X + offset + textDrawSz.Width > s.Bounds.Width ?
                    location.X - offset - textDrawSz.Width : location.X + offset,
                    location.Y + offset + textDrawSz.Height > s.Bounds.Height ?
                    location.Y - offset - textDrawSz.Height : location.Y + offset);

                //+Calculating area for writing selectable region dimensions 
                Rectangle rectTextArea = new Rectangle(textDrawPt, textDrawSz);
                Region rgTextArea = new Region(rectTextArea);

                graphics.FillRegion(Brushes.White, rgTextArea);
                graphics.DrawRectangle(Pens.Black, rectTextArea);

                // Write dimensions on the screen
                TextRenderer.DrawText(graphics, text, dimensions, textDrawPt, Color.Black, Color.Transparent);

                //// Exclud selected region area from opaque paint
                //graphics.ExcludeClip(rgTextArea);

                return rectTextArea;
            }
        }

        /// <summary>
        /// Gets small rectanlges from large 
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="node"></param>
        /// <param name="handleSize"></param>
        /// <returns></returns>
        public static PointF GetRectangleBounds(RectangleF rect, GrabHandle node, float handleSize)
        {
            float rectWidth = (float)rect.Width - 1;
            float rectHeight = (float)rect.Height - 1;
            float rectX = (float)rect.X;
            float rectY = (float)rect.Y;

            switch (node)
            {
                case GrabHandle.TopLeft:
                    return new PointF((rectX - handleSize / 2), (rectY - handleSize / 2));
                case GrabHandle.TopRight:
                    return new PointF(rectX + rectWidth - handleSize / 2, rectY - handleSize / 2);
                case GrabHandle.TopMid:
                    return new Point((int)(rectWidth - handleSize / 2) / 2 + (int)rectX, (int)(rectY - handleSize / 2));
                case GrabHandle.MidLeft:
                    return new PointF((int)(rectX - handleSize / 2), (int)(rectHeight / 2f + rectY - handleSize / 2f));
                case GrabHandle.MidRight:
                    return new Point((int)(rectX + rectWidth - handleSize / 2), (int)((rectHeight) / 2f + rectY - handleSize / 2f));
                case GrabHandle.BottomLeft:
                    return new PointF(rectX - handleSize / 2, rectY + rectHeight - handleSize / 2);
                case GrabHandle.BottomMid:
                    return new Point((int)((rectWidth - handleSize / 2) / 2 + rectX), (int)(rectY + rectHeight - handleSize / 2));
                case GrabHandle.BottomRight:
                    return new PointF(rectX + rectWidth - handleSize / 2, rectY + rectHeight - handleSize / 2);
            }

            return new PointF(0, 0);
        }        

        /// <summary>
        /// Overwrites image selection with 
        /// </summary>
        /// <param name="screenshot"></param>
        /// <param name="rect"></param>
        /// <returns></returns>
        public static Image SelectFromImage(Image screenshot, Rectangle rect)
        {
            var bmp = new Bitmap(screenshot);

            // Create the new bitmap and associated graphics object
            if (bmp != null)
            {
                bmp.Dispose();
                bmp = null;
            }

            bmp = new Bitmap(rect.Width, rect.Height);
            Graphics g = Graphics.FromImage(bmp);

            g.DrawImage(screenshot, 0, 0, rect, GraphicsUnit.Pixel);
            // Clean up
            g.Dispose();

            // Return the bitmap
            return bmp;
        }

        public static Rectangle ConvertRectFToRect(RectangleF rect)
        {
            var rectangle = new Rectangle();
            rectangle.X = (int)rect.X;
            rectangle.Y = (int)rect.Y;
            rectangle.Width = (int)rect.Width;
            rectangle.Height = (int)rect.Height;

            return rectangle;
        }
    }
}
