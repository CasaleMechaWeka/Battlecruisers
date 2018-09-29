using UnityEngine;

namespace BattleCruisers.UI.Cameras.InputHandlers
{
    /// <summary>
    /// Moves the camera towards where the mouse is.
    /// </summary>
    /// FELIX  Test
    /// FELIX  Use :P
    public class MouseLocationPositionFinder : IScrollPositionFinder
    {
        public Vector3 FindDesiredPosition(Vector3 cameraPosition, Vector3 mousePosition, float scrollSpeedInM)
        {
            return
                new Vector3(
                    FindDesiredValue(cameraPosition.x, mousePosition.x, scrollSpeedInM),
                    FindDesiredValue(cameraPosition.y, mousePosition.y, scrollSpeedInM),
                    cameraPosition.z);
        }

        private float FindDesiredValue(float currentValue, float targetValue, float maxChangeAllowed)
        {
            if (Mathf.Approximately(currentValue, targetValue))
            {
                return currentValue;
            }

            float intermediateValue = currentValue;
            float differenceInM = Mathf.Abs(currentValue - targetValue);
            float changeToUse = differenceInM > maxChangeAllowed ? maxChangeAllowed : differenceInM;

            if (intermediateValue < currentValue)
            {
                intermediateValue += changeToUse ;
            }
            else
            {
                intermediateValue -= changeToUse;
            }

            return intermediateValue;
        }
    }
}