using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.AngleLimiters
{
    public class AngleLimiter : IAngleLimiter
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
            // Don't create string messages to reduce memory usage
            Assert.IsTrue(desiredAngleInDegrees >= MIN_DESIRED_ANGLE_IN_DEGREES);  //, desiredAngleInDegrees + " should be >= " + MIN_DESIRED_ANGLE_IN_DEGREES);
            Assert.IsTrue(desiredAngleInDegrees <= MAX_DESIRED_ANGLE_IN_DEGREES);  //, desiredAngleInDegrees + " should be <= " + MAX_DESIRED_ANGLE_IN_DEGREES);

            // Convert from 0 > 360 to -180 to 180
            bool shouldConvert = desiredAngleInDegrees > 180;
            if (shouldConvert)
            {
                desiredAngleInDegrees -= 360;
            }

            float clampedAngle = Mathf.Clamp(desiredAngleInDegrees, _minAngle, _maxAngle);

            // Convert from -180 to 180 to 0 > 360
            if (shouldConvert)
            {
                Debug.Log($"clampedAngle: {clampedAngle}");
                clampedAngle += 360;
            }

            return clampedAngle;
        }
    }
}
