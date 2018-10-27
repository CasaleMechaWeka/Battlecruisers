using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils.Clamper
{
    /// <summary>
    /// Assumes the triangle:
    /// 1. Has a flat base
    /// 2. Has an apex between the two lower corners
    /// 
    /// Something like:
    ///       ^
    /// 
    /// <           >
    /// </summary>
    /// FELIX  Test :D
    public class TrianglePositionClamper : IPositionClamper
    {
        private Vector2 _bottomLeftVertex, _bottomeRightVertex, _topCenterVertex;
        private float _halfWidth, _height;

        public TrianglePositionClamper(Vector2 bottomLeftVertex, Vector2 bottomRightVertex, Vector2 topCenterVertex)
        {
            Helper.AssertIsNotNull(bottomLeftVertex, bottomRightVertex, topCenterVertex);

            Assert.IsTrue(bottomLeftVertex.x < topCenterVertex.x);
            Assert.IsTrue(topCenterVertex.x < bottomRightVertex.x);
            Assert.AreEqual(bottomLeftVertex.y, bottomRightVertex.y);
            Assert.IsTrue(topCenterVertex.y > bottomLeftVertex.y);

            _bottomLeftVertex = bottomLeftVertex;
            _bottomeRightVertex = bottomRightVertex;
            _topCenterVertex = topCenterVertex;

            _height = _topCenterVertex.y - _bottomeRightVertex.y;
            _halfWidth = (_bottomeRightVertex.x - _bottomLeftVertex.x) / 2;
        }

        public Vector3 Clamp(Vector3 position)
        {
            return Clamp((Vector2)position);
        }

        public Vector2 Clamp(Vector2 position)
        {
            float minX = _bottomLeftVertex.x;
            float maxX = _bottomeRightVertex.x;
            float clampedX = Mathf.Clamp(position.x, minX, maxX);

            float minY = _bottomLeftVertex.y;
            float maxY = FindMaxY(clampedX);
            float clampedY = Mathf.Clamp(position.y, minY, maxY);

            return new Vector2(clampedX, clampedY);
        }

        private float FindMaxY(float clampedX)
        {
            float proportionOfMaxHeight = _halfWidth - Mathf.Abs(_topCenterVertex.x - clampedX);
            float localMaxY = proportionOfMaxHeight / _halfWidth * _height;
            return _bottomLeftVertex.y + localMaxY;
        }
    }
}