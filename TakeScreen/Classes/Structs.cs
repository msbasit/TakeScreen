using System.Collections.Generic;
using System.Drawing;

namespace TakeScreen
{
    public struct Asset
    {
        /// <summary>
        /// Shape pf the asset
        /// </summary>
        public Shape Shape { get; set; }

        /// <summary>
        /// Drawing border width of asset
        /// </summary>
        public float BorderWidth { get; set; }

        /// <summary>
        /// Color of Asset
        /// </summary>
        public Color Color { get; set; }       

        /// <summary>
        /// Font size in case this is a text
        /// </summary>
        public float FontSize { get; set; }

        /// <summary>
        /// Font family in case this is a text 
        /// </summary>
        public FontFamily FontFamily { get; set; }

        /// <summary>
        /// Points in case this is a line
        /// </summary>
        public List<PointF> Points { get; set; }

        /// <summary>
        /// In case if this is a rectangle
        /// </summary>
        public RectangleF Rectangle { get; set; }

        /// <summary>
        /// Where the Pen point starts 
        /// </summary>
        public Point LineStart { get; set; }

        /// <summary>
        /// Where the Pen point ends
        /// </summary>
        public Point LineEnd { get; set; }                        
    }    
}
