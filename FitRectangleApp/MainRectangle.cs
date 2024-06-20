using System.Drawing;

namespace FitRectangleApp
{
    public class MainRectangle
    {
        public Point BottomLeft { get; set; }
        public Point TopRight { get; set; }
        public List<Rectangle> SubRectangles { get; set; }
        public List<string> Logs { get; set; }

        public MainRectangle(List<Rectangle> subRectangles, Point bottomLeft, Point topRight)
        {
            SubRectangles = subRectangles;
            Logs = new List<string>();
            BottomLeft = bottomLeft;
            TopRight = topRight;
        }

        public void LogToConsole()
        {
            foreach (var log in Logs)
            {
                Console.WriteLine(log);
            }
        }

        public void LogToFile(string filePath)
        {
            File.WriteAllLines(filePath, Logs);
        }

        public void SelectRectanglesByColor(List<Color> colors)
        {            
            SubRectangles = SubRectangles.Where(rectangle => colors.Contains(rectangle.Color)).ToList();
 
            Logs.Add($"Filtered rectangles by colors: {string.Join(", ", colors.Select(c => c.Name))}");
            CalculateBoundary();
        }

        public void RemoveRectanglesByColor(List<Color> colors)
        {
            int initialCount = SubRectangles.Count;
            SubRectangles.RemoveAll(r => colors.Contains(r.Color));
            int removedCount = initialCount - SubRectangles.Count;

            Logs.Add($"Removed {removedCount} rectangles of colors: {string.Join(", ", colors.Select(c => c.Name))}.");
            CalculateBoundary();
        }

        public void FilterRectanglesByBoundary() 
        {
            SubRectangles = SubRectangles.Where(r =>
                r.BottomLeft.X >= BottomLeft.X && r.BottomLeft.Y >= BottomLeft.Y &&
                r.TopRight.X <= TopRight.X && r.TopRight.Y <= TopRight.Y).ToList();

            Logs.Add("Filtered rectangles by boundary.");
            CalculateBoundary();
        }

        public void CalculateBoundary()
        {
            CalculateBoundaryCore();
        }

        public void CalculateIgnoreOutOfBoundary()
        {
            FilterRectanglesByBoundary();
            CalculateBoundaryCore();  
        }

        private void CalculateBoundaryCore()
        {
            if (SubRectangles.Count == 0)
            {
                BottomLeft = new Point(0, 0);
                TopRight = new Point(0, 0);
                return;
            }

            double minX = SubRectangles.Min(r => r.BottomLeft.X);
            double minY = SubRectangles.Min(r => r.BottomLeft.Y);
            double maxX = SubRectangles.Max(r => r.TopRight.X);
            double maxY = SubRectangles.Max(r => r.TopRight.Y);

            BottomLeft = new Point(minX, minY);
            TopRight = new Point(maxX, maxY);

            Logs.Add($"Boundary calculated: BottomLeft({BottomLeft.X}, {BottomLeft.Y}), TopRight({TopRight.X}, {TopRight.Y})");
        }
    }
}
