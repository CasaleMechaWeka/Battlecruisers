using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators
{
    public class AngleCalculator : IAngleCalculator
    {

        public virtual float FindDesiredAngle(Vector2 sourcePosition, Vector2 targetPosition, bool isSourceMirrored)
        {
            Assert.AreNotEqual(sourcePosition, targetPosition);

            //despite using an expensive Atan2 this is still ~5-10% faster than the old code and much more compact
            float angle = Mathf.Atan2(targetPosition.y - sourcePosition.y, targetPosition.x - sourcePosition.x) * Mathf.Rad2Deg;
            return isSourceMirrored ? 180 - angle : (angle + 360) % 360;
        }
    }
}
