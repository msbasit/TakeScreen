namespace TakeScreen.Forms
{
    partial class DrawingToolBox
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.colorpalette = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.text = new System.Windows.Forms.RadioButton();
            this.line = new System.Windows.Forms.RadioButton();
            this.linearrow = new System.Windows.Forms.RadioButton();
            this.circle = new System.Windows.Forms.RadioButton();
            this.rectangle = new System.Windows.Forms.RadioButton();
            this.path = new System.Windows.Forms.RadioButton();
            this.arrow = new System.Windows.Forms.RadioButton();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.colorpalette);
            this.panel1.Controls.Add(this.text);
            this.panel1.Controls.Add(this.line);
            this.panel1.Controls.Add(this.linearrow);
            this.panel1.Controls.Add(this.circle);
            this.panel1.Controls.Add(this.rectangle);
            this.panel1.Controls.Add(this.path);
            this.panel1.Controls.Add(this.arrow);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(2);
            this.panel1.Size = new System.Drawing.Size(32, 249);
            this.panel1.TabIndex = 1;
            // 
            // colorpalette
            // 
            this.colorpalette.BackColor = System.Drawing.Color.Red;
            this.colorpalette.Dock = System.Windows.Forms.DockStyle.Top;
            this.colorpalette.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.colorpalette.FlatAppearance.BorderSize = 5;
            this.colorpalette.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.colorpalette.Location = new System.Drawing.Point(2, 191);
            this.colorpalette.Name = "colorpalette";
            this.colorpalette.Size = new System.Drawing.Size(26, 27);
            this.colorpalette.TabIndex = 16;
            this.colorpalette.UseVisualStyleBackColor = false;
            this.colorpalette.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Silver;
            this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Image = global::TakeScreen.Properties.Resources.Close;
            this.btnCancel.Location = new System.Drawing.Point(2, 218);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(26, 27);
            this.btnCancel.TabIndex = 17;
            this.btnCancel.UseVisualStyleBackColor = false;
            // 
            // text
            // 
            this.text.Appearance = System.Windows.Forms.Appearance.Button;
            this.text.AutoEllipsis = true;
            this.text.BackColor = System.Drawing.Color.Transparent;
            this.text.Dock = System.Windows.Forms.DockStyle.Top;
            this.text.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.text.FlatAppearance.BorderSize = 0;
            this.text.FlatAppearance.CheckedBackColor = System.Drawing.Color.Silver;
            this.text.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightGray;
            this.text.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gainsboro;
            this.text.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.text.Image = global::TakeScreen.Properties.Resources.Text;
            this.text.Location = new System.Drawing.Point(2, 164);
            this.text.Name = "text";
            this.text.Size = new System.Drawing.Size(26, 27);
            this.text.TabIndex = 15;
            this.text.UseVisualStyleBackColor = false;
            this.text.CheckedChanged += new System.EventHandler(this.text_CheckedChanged);
            // 
            // line
            // 
            this.line.Appearance = System.Windows.Forms.Appearance.Button;
            this.line.AutoEllipsis = true;
            this.line.BackColor = System.Drawing.Color.Transparent;
            this.line.Dock = System.Windows.Forms.DockStyle.Top;
            this.line.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.line.FlatAppearance.BorderSize = 0;
            this.line.FlatAppearance.CheckedBackColor = System.Drawing.Color.Silver;
            this.line.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightGray;
            this.line.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gainsboro;
            this.line.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.line.Image = global::TakeScreen.Properties.Resources.line;
            this.line.Location = new System.Drawing.Point(2, 137);
            this.line.Name = "line";
            this.line.Size = new System.Drawing.Size(26, 27);
            this.line.TabIndex = 14;
            this.line.UseVisualStyleBackColor = false;
            this.line.CheckedChanged += new System.EventHandler(this.line_CheckedChanged);
            // 
            // linearrow
            // 
            this.linearrow.Appearance = System.Windows.Forms.Appearance.Button;
            this.linearrow.AutoEllipsis = true;
            this.linearrow.BackColor = System.Drawing.Color.Transparent;
            this.linearrow.Dock = System.Windows.Forms.DockStyle.Top;
            this.linearrow.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.linearrow.FlatAppearance.BorderSize = 0;
            this.linearrow.FlatAppearance.CheckedBackColor = System.Drawing.Color.Silver;
            this.linearrow.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightGray;
            this.linearrow.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gainsboro;
            this.linearrow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.linearrow.Image = global::TakeScreen.Properties.Resources.arrowline;
            this.linearrow.Location = new System.Drawing.Point(2, 110);
            this.linearrow.Name = "linearrow";
            this.linearrow.Size = new System.Drawing.Size(26, 27);
            this.linearrow.TabIndex = 13;
            this.linearrow.UseVisualStyleBackColor = false;
            this.linearrow.CheckedChanged += new System.EventHandler(this.linearrow_CheckedChanged);
            // 
            // circle
            // 
            this.circle.Appearance = System.Windows.Forms.Appearance.Button;
            this.circle.AutoEllipsis = true;
            this.circle.BackColor = System.Drawing.Color.Transparent;
            this.circle.Dock = System.Windows.Forms.DockStyle.Top;
            this.circle.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.circle.FlatAppearance.BorderSize = 0;
            this.circle.FlatAppearance.CheckedBackColor = System.Drawing.Color.Silver;
            this.circle.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightGray;
            this.circle.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gainsboro;
            this.circle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.circle.Image = global::TakeScreen.Properties.Resources.Cirkil;
            this.circle.Location = new System.Drawing.Point(2, 83);
            this.circle.Name = "circle";
            this.circle.Size = new System.Drawing.Size(26, 27);
            this.circle.TabIndex = 12;
            this.circle.UseVisualStyleBackColor = false;
            this.circle.CheckedChanged += new System.EventHandler(this.circle_CheckedChanged);
            // 
            // rectangle
            // 
            this.rectangle.Appearance = System.Windows.Forms.Appearance.Button;
            this.rectangle.AutoEllipsis = true;
            this.rectangle.BackColor = System.Drawing.Color.Transparent;
            this.rectangle.Dock = System.Windows.Forms.DockStyle.Top;
            this.rectangle.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.rectangle.FlatAppearance.BorderSize = 0;
            this.rectangle.FlatAppearance.CheckedBackColor = System.Drawing.Color.Silver;
            this.rectangle.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightGray;
            this.rectangle.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gainsboro;
            this.rectangle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rectangle.Image = global::TakeScreen.Properties.Resources.acute_rect;
            this.rectangle.Location = new System.Drawing.Point(2, 56);
            this.rectangle.Name = "rectangle";
            this.rectangle.Size = new System.Drawing.Size(26, 27);
            this.rectangle.TabIndex = 11;
            this.rectangle.UseVisualStyleBackColor = false;
            this.rectangle.CheckedChanged += new System.EventHandler(this.rectangle_CheckedChanged);
            // 
            // path
            // 
            this.path.Appearance = System.Windows.Forms.Appearance.Button;
            this.path.AutoEllipsis = true;
            this.path.BackColor = System.Drawing.Color.Transparent;
            this.path.Dock = System.Windows.Forms.DockStyle.Top;
            this.path.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.path.FlatAppearance.BorderSize = 0;
            this.path.FlatAppearance.CheckedBackColor = System.Drawing.Color.Silver;
            this.path.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightGray;
            this.path.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gainsboro;
            this.path.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.path.Image = global::TakeScreen.Properties.Resources.Pen;
            this.path.Location = new System.Drawing.Point(2, 29);
            this.path.Name = "path";
            this.path.Size = new System.Drawing.Size(26, 27);
            this.path.TabIndex = 10;
            this.path.UseVisualStyleBackColor = false;
            this.path.CheckedChanged += new System.EventHandler(this.path_CheckedChanged);
            // 
            // arrow
            // 
            this.arrow.Appearance = System.Windows.Forms.Appearance.Button;
            this.arrow.AutoEllipsis = true;
            this.arrow.BackColor = System.Drawing.Color.Transparent;
            this.arrow.Checked = true;
            this.arrow.Dock = System.Windows.Forms.DockStyle.Top;
            this.arrow.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.arrow.FlatAppearance.BorderSize = 0;
            this.arrow.FlatAppearance.CheckedBackColor = System.Drawing.Color.Silver;
            this.arrow.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightGray;
            this.arrow.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gainsboro;
            this.arrow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.arrow.Image = global::TakeScreen.Properties.Resources.pointer;
            this.arrow.Location = new System.Drawing.Point(2, 2);
            this.arrow.Name = "arrow";
            this.arrow.Size = new System.Drawing.Size(26, 27);
            this.arrow.TabIndex = 9;
            this.arrow.TabStop = true;
            this.arrow.UseVisualStyleBackColor = false;
            this.arrow.CheckedChanged += new System.EventHandler(this.arrow_CheckedChanged);
            // 
            // DrawingToolBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(32, 249);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "DrawingToolBox";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "DrawingToolBox";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton arrow;
        private System.Windows.Forms.RadioButton text;
        private System.Windows.Forms.RadioButton line;
        private System.Windows.Forms.RadioButton linearrow;
        private System.Windows.Forms.RadioButton circle;
        private System.Windows.Forms.RadioButton rectangle;
        private System.Windows.Forms.RadioButton path;
        private System.Windows.Forms.Button colorpalette;
        public System.Windows.Forms.Button btnCancel;
    }
}