using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.AngleLimiters
{
    public class AngleLimiter : IAngleLimiter
    {
        private readonly float _minAngle;
        private readonly float _maxAngle;

        public AngleLimiter(float minAngle, float maxAngle)
        {
            Assert.IsTrue(minAngle < maxAngle);

            _minAngle = minAngle;
            _maxAngle = maxAngle;
        }

        public float LimitAngle(float desiredAngleInDegrees)
        {
            return Mathf.Clamp(desiredAngleInDegrees, _minAngle, _maxAngle);
        }
    }
}
