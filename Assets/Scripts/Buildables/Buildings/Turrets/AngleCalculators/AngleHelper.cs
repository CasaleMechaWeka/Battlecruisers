using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators
{
    public class AngleHelper : IAngleHelper
    {
        public float FindAngle(Vector2 velocity, bool isSourceMirrored)
        {
            Vector2 source = Vector2.zero;
            Vector2 target = velocity;

            return FindAngle(source, target, isSourceMirrored);
        }

        public float FindAngle(Vector2 sourcePosition, Vector2 targetPosition, bool isSourceMirrored)
        {
            Assert.AreNotEqual(sourcePosition, targetPosition);

            //despite using an expensive Atan2 this is still ~5-10% faster than the old code and much more compact
            float angle = Mathf.Atan2(targetPosition.y - sourcePosition.y, targetPosition.x - sourcePosition.x) * Mathf.Rad2Deg;
            return isSourceMirrored ? 180 - angle : (angle + 360) % 360;
        }
    }
}
