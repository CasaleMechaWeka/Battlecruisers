using UnityEngine;

namespace BattleCruisers.Movement.Rotation
{
    public class RotationHelper : IRotationHelper
    {
        /// <returns>
        /// 1 if it is shorter to rotate anti-clockwise, -1 if it is shorter to rotate clockwise, 
        /// 0 if the desired angle is the same as the current angle.
        /// </returns>
        public float FindDirectionMultiplier(float currentAngleInRadians, float desiredAngleInDegrees)
        {
            if (currentAngleInRadians == desiredAngleInDegrees)
            {
                return 0;
            }

            float distance = Mathf.Abs(currentAngleInRadians - desiredAngleInDegrees);

            if (desiredAngleInDegrees > currentAngleInRadians)
            {
                return distance < 180 ? 1 : -1;
            }
            else
            {
                return distance < 180 ? -1 : 1;
            }
        }
    }
}