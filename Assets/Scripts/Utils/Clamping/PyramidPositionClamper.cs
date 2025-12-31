using BattleCruisers.Utils.DataStrctures;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils.Clamping
{
    public class PyramidPositionClamper : IPositionClamper
    {
        private readonly IPyramid _pyramid;

        public PyramidPositionClamper(IPyramid pyramid)
        {
            Assert.IsNotNull(pyramid);
            _pyramid = pyramid;
        }

        public Vector3 Clamp(Vector3 position)
        {
            return Clamp((Vector2)position);
        }

        public Vector2 Clamp(Vector2 position)
        {
            float minX = _pyramid.BottomLeftVertex.x;
            float maxX = _pyramid.BottomRightVertex.x;
            float clampedX = Mathf.Clamp(position.x, minX, maxX);

            Logging.Verbose(Tags.PYRAMID, $"originalX: {position.x}  clampedX: {clampedX}  minX: {minX}  maxX: {maxX}");

            float minY = _pyramid.BottomLeftVertex.y;
            float maxY = _pyramid.FindMaxY(clampedX);
            float clampedY = Mathf.Clamp(position.y, minY, maxY);

            return new Vector2(clampedX, clampedY);
        }
    }
}