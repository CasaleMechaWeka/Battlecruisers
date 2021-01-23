using BattleCruisers.Utils;
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

            float desiredAngleInDegrees;

            if (sourcePosition.x == targetPosition.x)
            {
                // On same x-axis
                desiredAngleInDegrees = sourcePosition.y < targetPosition.y ? 90 : 270;
            }
            else if (sourcePosition.y == targetPosition.y)
            {
                // On same y-axis
                if (sourcePosition.x < targetPosition.x)
                {
                    desiredAngleInDegrees = isSourceMirrored ? 180 : 0;
                }
                else
                {
                    desiredAngleInDegrees = isSourceMirrored ? 0 : 180;
                }
            }
            else
            {
                // Different x and y axes, so need to calculate the angle
                float xDiff = Mathf.Abs(sourcePosition.x - targetPosition.x);
                float yDiff = Mathf.Abs(sourcePosition.y - targetPosition.y);
                float angleInDegrees = Mathf.Atan(yDiff / xDiff) * Mathf.Rad2Deg;
                Logging.Verbose(Tags.ANGLE_CALCULATORS, "angleInDegrees: " + angleInDegrees);

                if (sourcePosition.x < targetPosition.x)
                {
                    // Source is to left of target
                    if (sourcePosition.y < targetPosition.y)
                    {
                        // Source is below target
                        desiredAngleInDegrees = isSourceMirrored ? 180 - angleInDegrees : angleInDegrees;
                    }
                    else
                    {
                        // Source is above target
                        desiredAngleInDegrees = isSourceMirrored ? 180 + angleInDegrees : 360 - angleInDegrees;
                    }
                }
                else
                {
                    // Source is to right of target
                    if (sourcePosition.y < targetPosition.y)
                    {
                        // Source is below target
                        desiredAngleInDegrees = isSourceMirrored ? angleInDegrees : 180 - angleInDegrees;
                    }
                    else
                    {
                        // Source is above target
                        desiredAngleInDegrees = isSourceMirrored ? 360 - angleInDegrees : 180 + angleInDegrees;
                    }
                }
            }

            Logging.Verbose(Tags.ANGLE_CALCULATORS, desiredAngleInDegrees + "*");
            return desiredAngleInDegrees;
        }
    }
}
