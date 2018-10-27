using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.Navigation
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
    public class TrianglePositionValidator : IPositionValidator
    {
        private Vector2 _bottomLeftVertex, _bottomeRightVertex, _topCenterVertex;
        private float _halfWidth, _height;

        public TrianglePositionValidator(Vector2 bottomLeftVertex, Vector2 bottomRightVertex, Vector2 topCenterVertex)
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

        public bool IsValid(Vector2 position)
        {
            return
                position.x >= _bottomLeftVertex.x
                && position.x <= _bottomeRightVertex.x
                && position.y >= _bottomLeftVertex.y
                && position.y <= FindMaxY(position);
        }

        private float FindMaxY(Vector2 position)
        {
            return Mathf.Abs(_topCenterVertex.x - position.x) / _halfWidth * _height;
        }
    }
}