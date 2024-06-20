using System.Drawing;

namespace FitRectangleApp
{
    public class Rectangle
    {
        public Color Color { get; set; }
        public Point BottomLeft { get; set; }
        public Point TopRight { get; set; }

        public Rectangle(Color color, Point bottomLeft, Point topRight)
        {
            Color = color;
            BottomLeft = bottomLeft;
            TopRight = topRight;
        }
    }
}