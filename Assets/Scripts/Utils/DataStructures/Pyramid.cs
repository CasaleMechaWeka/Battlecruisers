using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils.DataStrctures
{
    public class Pyramid : IPyramid
    {
        public Vector2 BottomLeftVertex { get; private set; }
        public Vector2 BottomRightVertex { get; private set; }
        public Vector2 TopCenterVertex { get; private set; }
        public float Width { get; private set; }
        public float Height { get; private set; }

        // FELIX  Test!  (Including asserts)
        public Pyramid(Vector2 bottomLeftVertex, Vector2 bottomRightVertex, Vector2 topCenterVertex)
        {
            Assert.IsTrue(bottomLeftVertex.x < topCenterVertex.x);
            Assert.IsTrue(topCenterVertex.x < bottomRightVertex.x);
            Assert.AreEqual(bottomLeftVertex.y, bottomRightVertex.y);
            Assert.IsTrue(topCenterVertex.y > bottomLeftVertex.y);
            Assert.IsTrue(Mathf.Approximately(topCenterVertex.x - bottomLeftVertex.x, bottomRightVertex.x - topCenterVertex.x));

            BottomLeftVertex = bottomLeftVertex;
            BottomRightVertex = bottomRightVertex;
            TopCenterVertex = topCenterVertex;

            Height = TopCenterVertex.y - BottomLeftVertex.y;
            Width = BottomRightVertex.x - BottomLeftVertex.x;
        }

        public float FindMaxY(float xPosition)
        {
            Assert.IsTrue(xPosition >= BottomLeftVertex.x);
            Assert.IsTrue(xPosition <= BottomRightVertex.x);

            float halfWidth = Width / 2;
            float proportionOfMaxHeight = halfWidth - Mathf.Abs(TopCenterVertex.x - xPosition);
            float localMaxY = proportionOfMaxHeight / halfWidth * Height;
            return BottomLeftVertex.y + localMaxY;
        }

        public IRange<float> FindGlobalXRange(float yPosition)
        {
            Assert.IsTrue(yPosition >= 0);
            Assert.IsTrue(yPosition <= Height);

            float proportionOfHeight = yPosition / Height;
            float widthAtHeight = (1 - proportionOfHeight) * Width;
            float halfWidth = widthAtHeight / 2;

            float xMin = TopCenterVertex.x - halfWidth;
            float xMax = TopCenterVertex.x + halfWidth;
            return new Range<float>(xMin, xMax);
        }
    }
}