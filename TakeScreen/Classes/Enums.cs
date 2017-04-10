namespace TakeScreen
{
    public enum GrabHandle
    {
        None,
        TopLeft,
        TopMid,
        TopRight,
        MidLeft,
        MidRight,
        BottomLeft,
        BottomMid,
        BottomRight
    }

    public enum RectangleState
    {
        None,
        Draw,
        Resize,
        Move
    }

    public enum DrawingTool
    {
        Arrow,
        Pen,
        Line,
        Rectangle,
        Circle,
        Text,
        ArrowLine
    }

    public enum Shape
    {
        Rectangle,
        Line,
        ArrowLine,
        Circle,
        Text,
        Pen
    }
}
