using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils.DataStrctures
{
    public class Pyramid : IPyramid
    {
        private readonly float _height;
        private readonly float _width;

        public Vector2 BottomLeftVertex { get; private set; }
        public Vector2 BottomRightVertex { get; private set; }
        public Vector2 TopCenterVertex { get; private set; }

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

            _height = TopCenterVertex.y - BottomLeftVertex.y;
            _width = BottomRightVertex.x - BottomLeftVertex.x;
        }

        public float FindMaxY(float xPosition)
        {
            Assert.IsTrue(xPosition >= BottomLeftVertex.x);
            Assert.IsTrue(xPosition <= BottomRightVertex.x);

            float halfWidth = _width / 2;
            float proportionOfMaxHeight = halfWidth - Mathf.Abs(TopCenterVertex.x - xPosition);
            float localMaxY = proportionOfMaxHeight / halfWidth * _height;
            return BottomLeftVertex.y + localMaxY;
        }
    }
}