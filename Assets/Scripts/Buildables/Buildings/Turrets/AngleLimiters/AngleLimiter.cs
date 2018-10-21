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

		private const float MIN_DESIRED_ANGLE_IN_DEGREES = -360;
        private const float MAX_DESIRED_ANGLE_IN_DEGREES = 360;

        public AngleLimiter(float minAngle, float maxAngle)
        {
            Assert.IsTrue(minAngle < maxAngle);
            Assert.IsTrue(minAngle >= MIN_MIN_ANGLE);
            Assert.IsTrue(maxAngle <= MAX_MAX_ANGLE);

            _minAngle = minAngle;
            _maxAngle = maxAngle;
        }

        /// <param name="desiredAngleInDegrees">
        /// // FELIX  Update comment :/
        /// Desired angle in degrees: -360 to 360
        /// </param>
        /// FELIX  Update tests :)
        public float LimitAngle(float desiredAngleInDegrees)
        {
            Assert.IsTrue(desiredAngleInDegrees >= MIN_DESIRED_ANGLE_IN_DEGREES);
            Assert.IsTrue(desiredAngleInDegrees <= MAX_DESIRED_ANGLE_IN_DEGREES);

            // Convert too large angles to negative angles
            if (desiredAngleInDegrees > 180)
            {
                desiredAngleInDegrees = desiredAngleInDegrees - 360;
            }

            float clampedAngle = Mathf.Clamp(desiredAngleInDegrees, _minAngle, _maxAngle);

            // Convert negative angles to large angles
            if (clampedAngle < 0)
            {
                clampedAngle = clampedAngle + 360;
            }

            return clampedAngle;
        }
    }
}
