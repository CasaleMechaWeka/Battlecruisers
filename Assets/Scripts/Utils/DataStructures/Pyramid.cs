using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils.DataStrctures
{
    public class Pyramid : IPyramid
    {
        public Vector2 BottomLeftVertex { get; }
        public Vector2 BottomRightVertex { get; }
        public Vector2 TopCenterVertex { get; }
        public float Width { get; }
        public float Height { get; }

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

        public float FindMaxY(float globalXPosition)
        {
            Logging.Verbose(Tags.PYRAMID, $"globalXPosition: {globalXPosition}  BottomLeftVertex.x: {BottomLeftVertex.x}  BottomRightVertex.x): {BottomRightVertex.x}");
            Assert.IsTrue(globalXPosition >= BottomLeftVertex.x);
            Assert.IsTrue(globalXPosition <= BottomRightVertex.x);

            float halfWidth = Width / 2;
            float proportionOfMaxHeight = halfWidth - Mathf.Abs(TopCenterVertex.x - globalXPosition);
            float localMaxY = proportionOfMaxHeight / halfWidth * Height;
            return BottomLeftVertex.y + localMaxY;
        }

        public IRange<float> FindGlobalXRange(float localYPosition)
        {
            Assert.IsTrue(localYPosition >= 0);
            Assert.IsTrue(localYPosition <= Height);

            float proportionOfHeight = localYPosition / Height;
            float widthAtHeight = (1 - proportionOfHeight) * Width;
            float halfWidth = widthAtHeight / 2;

            float xMin = TopCenterVertex.x - halfWidth;
            float xMax = TopCenterVertex.x + halfWidth;
            return new Range<float>(xMin, xMax);
        }
    }
}