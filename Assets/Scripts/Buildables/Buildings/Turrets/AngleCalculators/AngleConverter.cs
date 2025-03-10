using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators
{
    public class AngleConverter : IAngleConverter
    {
        private const float MIN_UNSIGNED_ANGLE_IN_DEGREES = 0;
        private const float MAX_UNSIGNED_ANGLE_IN_DEGREES = 360;
        private const float MIN_SIGNED_ANGLE_IN_DEGREES = -180;
        private const float MAX_SIGNED_ANGLE_IN_DEGREES = 180;

        public float ConvertToSigned(float unsignedAngleInDegrees)
        {
            Assert.IsTrue(unsignedAngleInDegrees >= MIN_UNSIGNED_ANGLE_IN_DEGREES);
            Assert.IsTrue(unsignedAngleInDegrees <= MAX_UNSIGNED_ANGLE_IN_DEGREES);

            if (unsignedAngleInDegrees > 180)
            {
                unsignedAngleInDegrees -= 360;
            }

            return unsignedAngleInDegrees;
        }

        public float ConvertToUnsigned(float signedAngleInDegrees)
        {
            Assert.IsTrue(signedAngleInDegrees >= MIN_SIGNED_ANGLE_IN_DEGREES);
            Assert.IsTrue(signedAngleInDegrees <= MAX_SIGNED_ANGLE_IN_DEGREES);

            if (signedAngleInDegrees < 0)
            {
                signedAngleInDegrees += 360;
            }

            return signedAngleInDegrees;
        }
    }
}