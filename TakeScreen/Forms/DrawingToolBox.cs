using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace TakeScreen.Forms
{
    public partial class DrawingToolBox : Form
	{
		public DrawingToolBox()
		{
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            UpdateStyles();
            InitializeComponent();

            typeof(Panel).InvokeMember("DoubleBuffered", BindingFlags.SetProperty
            | BindingFlags.Instance | BindingFlags.NonPublic, null,
            arrow, new object[] { true });

            typeof(Panel).InvokeMember("DoubleBuffered", BindingFlags.SetProperty
            | BindingFlags.Instance | BindingFlags.NonPublic, null,
            path, new object[] { true });

            typeof(Panel).InvokeMember("DoubleBuffered", BindingFlags.SetProperty
            | BindingFlags.Instance | BindingFlags.NonPublic, null,
            line, new object[] { true });

            typeof(Panel).InvokeMember("DoubleBuffered", BindingFlags.SetProperty
            | BindingFlags.Instance | BindingFlags.NonPublic, null,
            linearrow, new object[] { true });

            typeof(Panel).InvokeMember("DoubleBuffered", BindingFlags.SetProperty
            | BindingFlags.Instance | BindingFlags.NonPublic, null,
            panel1, new object[] { true });

            BackColor = Color.Transparent;
        }

        private void button1_Click(object sender, EventArgs e)
		{
			using (var cld = new ColorDialog())
			{
				if (cld.ShowDialog() == DialogResult.OK)
				{
					colorpalette.BackColor = cld.Color;
					selectedColor = cld.Color;
				}
			}
		}

		public event DrawingToolChanged ToolChanged;

		private DrawingTool currentDrawingTool = DrawingTool.Arrow;

		public DrawingTool CurrentDrawingTool
		{
			get { return currentDrawingTool; }
		}

		private Color selectedColor = Color.Red;

		public Color SelectedColor
		{
			get { return selectedColor; }
		}

		private void arrow_CheckedChanged(object sender, EventArgs e)
		{
			currentDrawingTool = arrow.Checked ? DrawingTool.Arrow : currentDrawingTool;
            ToolChanged?.Invoke(currentDrawingTool);            
        }

		private void path_CheckedChanged(object sender, EventArgs e)
		{
			currentDrawingTool = path.Checked ? DrawingTool.Pen : currentDrawingTool;
            ToolChanged?.Invoke(currentDrawingTool);
        }

		private void rectangle_CheckedChanged(object sender, EventArgs e)
		{
			currentDrawingTool = rectangle.Checked ? DrawingTool.Rectangle : currentDrawingTool;
            ToolChanged?.Invoke(currentDrawingTool);
        }

		private void circle_CheckedChanged(object sender, EventArgs e)
		{
			currentDrawingTool = circle.Checked ? DrawingTool.Circle : currentDrawingTool;
            ToolChanged?.Invoke(currentDrawingTool);
        }

		private void linearrow_CheckedChanged(object sender, EventArgs e)
		{
			currentDrawingTool = linearrow.Checked ? DrawingTool.ArrowLine : currentDrawingTool;
            ToolChanged?.Invoke(currentDrawingTool);
		}

		private void line_CheckedChanged(object sender, EventArgs e)
		{
			currentDrawingTool = line.Checked ? DrawingTool.Line : currentDrawingTool;
            ToolChanged?.Invoke(currentDrawingTool);
        }

		private void text_CheckedChanged(object sender, EventArgs e)
		{
			currentDrawingTool = text.Checked ? DrawingTool.Text : currentDrawingTool;
            ToolChanged?.Invoke(currentDrawingTool);
        }

        internal void ResetDrawingTool()
        {            
            arrow.Checked = true;   
        }
    }
}
