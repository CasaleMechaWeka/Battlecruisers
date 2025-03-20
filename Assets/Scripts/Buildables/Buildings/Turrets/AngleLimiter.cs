using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.AngleLimiters
{
    public class AngleLimiter
    {
        private readonly float _minAngle;
        private readonly float _maxAngle;

        private const float MIN_MIN_ANGLE = -180;
        private const float MAX_MAX_ANGLE = 180;

        private const float MIN_DESIRED_ANGLE_IN_DEGREES = 0;
        private const float MAX_DESIRED_ANGLE_IN_DEGREES = 360;

        public AngleLimiter(float minAngle, float maxAngle)
        {
            Assert.IsTrue(minAngle < maxAngle);
            Assert.IsTrue(minAngle >= MIN_MIN_ANGLE);
            Assert.IsTrue(maxAngle <= MAX_MAX_ANGLE);

            _minAngle = minAngle;
            _maxAngle = maxAngle;
        }

        /// <summary>
        /// NOTE:
        /// desiredAngleInDegrees: 0 to 360
        /// allowed range:  -180 to 180
        /// 
        /// The allowed range is specified thus to allow facing angle limiters to be
        /// specified easily.  Ie:
        ///     -90 to 90
        /// Instead of:
        ///     0 to 90 AND 270 to 360
        /// </summary>
        public float LimitAngle(float desiredAngleInDegrees)
        {
            Assert.IsTrue(desiredAngleInDegrees >= MIN_DESIRED_ANGLE_IN_DEGREES);
            Assert.IsTrue(desiredAngleInDegrees <= MAX_DESIRED_ANGLE_IN_DEGREES);

            float adjustedAngle = desiredAngleInDegrees > 180 ? desiredAngleInDegrees - 360 : desiredAngleInDegrees;
            return (adjustedAngle = Mathf.Clamp(adjustedAngle, _minAngle, _maxAngle)) < 0 ? adjustedAngle + 360 : adjustedAngle;
        }
    }
}
