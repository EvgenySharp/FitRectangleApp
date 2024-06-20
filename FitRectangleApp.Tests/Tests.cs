using System.Drawing;
using Xunit;

namespace FitRectangleApp.Tests
{
    public class Tests
    {
        [Fact]
        public void CalculateBoundary_ShouldCalculateCorrectly()
        {
            var subRectangles = new List<Rectangle>
            {
                new Rectangle(Color.Green, new Point(1, 1), new Point(3, 3)),
                new Rectangle(Color.Red, new Point(2, 2), new Point(4, 4))
            };
            var mainRectangle = new MainRectangle(subRectangles, new Point(1, 1), new Point(3, 3));

            mainRectangle.CalculateBoundary();

            Assert.Equal(new Point(1, 1), mainRectangle.BottomLeft);
            Assert.Equal(new Point(4, 4), mainRectangle.TopRight);
        }

        [Fact]
        public void CalculateIgnoreOutOfBoundary_ShouldCalculateCorrectly()
        {
            var subRectangles = new List<Rectangle>
            {
                new Rectangle(Color.Green, new Point(1, 1), new Point(3, 3)),
                new Rectangle(Color.Red, new Point(2, 2), new Point(4, 4)),
                new Rectangle(Color.Green, new Point(0, 0), new Point(5, 5))
            };
            var mainRectangle = new MainRectangle(subRectangles, new Point(0, 0), new Point(3, 3));

            mainRectangle.CalculateIgnoreOutOfBoundary();

            Assert.Equal(new Point(1, 1), mainRectangle.BottomLeft);
            Assert.Equal(new Point(3, 3), mainRectangle.TopRight);
        }

        [Fact]
        public void FilterRectanglesByColor_ShouldFilterCorrectly()
        {
            var subRectangles = new List<Rectangle>
            {
                new Rectangle(Color.Green, new Point(1, 1), new Point(3, 3)),
                new Rectangle(Color.Red, new Point(2, 2), new Point(4, 4))
            };
            var mainRectangle = new MainRectangle(subRectangles, new Point(1, 1), new Point(3, 3));

            mainRectangle.CalculateBoundary();
            mainRectangle.SelectRectanglesByColor(new List<Color> { Color.Green });

            Assert.Single(mainRectangle.SubRectangles);
            Assert.Equal(Color.Green, mainRectangle.SubRectangles[0].Color);
        }

        [Fact]
        public void FilterRectanglesByBoundary_ShouldFilterCorrectly()
        {
            var subRectangles = new List<Rectangle>
            {
                new Rectangle(Color.Green, new Point(1, 1), new Point(3, 3)),
                new Rectangle(Color.Red, new Point(0, 0), new Point(5, 5))
            };
            var mainRectangle = new MainRectangle(subRectangles, new Point(1, 1), new Point(3, 3));

            mainRectangle.CalculateBoundary();
            mainRectangle.FilterRectanglesByBoundary();

            Assert.Equal(2, mainRectangle.SubRectangles.Count);
        }

        [Fact]
        public void RemoveRectanglesByColor_ShouldRemoveCorrectly()
        {
            var subRectangles = new List<Rectangle>
            {
                new Rectangle(Color.Green, new Point(1, 1), new Point(3, 3)),
                new Rectangle(Color.Red, new Point(2, 2), new Point(4, 4))
            };
            var mainRectangle = new MainRectangle(subRectangles, new Point(1, 1), new Point(3, 3));

            mainRectangle.CalculateBoundary();
            mainRectangle.RemoveRectanglesByColor(new List<Color> { Color.Green });

            Assert.Single(mainRectangle.SubRectangles);
            Assert.Equal(Color.Red, mainRectangle.SubRectangles[0].Color);
        }

        [Fact]
        public void FilterRectanglesByColorAndBoundaryAndCalculateIgnoreOutOfBoundary_ShouldFilterCorrectly()
        {
            var subRectangles = new List<Rectangle>
            {
                new Rectangle(Color.Green, new Point(1, 1), new Point(3, 3)),
                new Rectangle(Color.Red, new Point(2, 2), new Point(4, 4)),
                new Rectangle(Color.Green, new Point(0, 0), new Point(5, 5))
            };
            var mainRectangle = new MainRectangle(subRectangles, new Point(0, 0), new Point(3, 3));

            mainRectangle.CalculateIgnoreOutOfBoundary();
            mainRectangle.SelectRectanglesByColor(new List<Color> { Color.Green });
            mainRectangle.FilterRectanglesByBoundary();

            Assert.Single(mainRectangle.SubRectangles);
            Assert.Equal(new Point(1, 1), mainRectangle.SubRectangles[0].BottomLeft);
            Assert.Equal(new Point(3, 3), mainRectangle.SubRectangles[0].TopRight);
        }

        [Fact]
        public void LogToFile()
        {
            var subRectangles = new List<Rectangle>
                {
                    new Rectangle(Color.Green, new Point(1, 1), new Point(3, 3)),
                    new Rectangle(Color.Red, new Point(2, 2), new Point(4, 4)),
                    new Rectangle(Color.Blue, new Point(5, 5), new Point(6, 6))
                };

            var mainRectangle = new MainRectangle(subRectangles, new Point(1, 1), new Point(5, 5));
            mainRectangle.Logs.AddRange(new List<string> { "Test log entry 1", "Test log entry 2", "Test log entry 3" });
            string filePath = "test_log.txt";

            mainRectangle.LogToFile(filePath);

            Assert.True(File.Exists(filePath), "Log file was not created.");
            var loggedLines = File.ReadAllLines(filePath);
            Assert.Equal(mainRectangle.Logs.Count, loggedLines.Length);

            for (int i = 0; i < mainRectangle.Logs.Count; i++)
            {
                Assert.Equal(mainRectangle.Logs[i], loggedLines[i]);
            }

            File.Delete(filePath);
        }
    }
}
