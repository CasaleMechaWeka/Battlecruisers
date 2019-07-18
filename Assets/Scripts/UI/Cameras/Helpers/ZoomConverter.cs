using BattleCruisers.Data.Settings;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Cameras.Helpers
{
    public class ZoomConverter : ILevelToMultiplierConverter
    {
        // Perhaps there is some fancy mathematical formula I could use :P
        public float LevelToMultiplier(int zoomLevel)
        {
            Assert.IsTrue(zoomLevel >= SettingsManager.MIN_ZOOM_SPEED_LEVEL);
            Assert.IsTrue(zoomLevel <= SettingsManager.MAX_ZOOM_SPEED_LEVEL);

            switch (zoomLevel)
            {
                case 1:
                    return 0.1f;

                case 2:
                    return 0.2f;

                case 3:
                    return 0.4f;

                case 4:
                    return 0.8f;

                // Default settings
                case 5:
                    return 1;

                case 6:
                    return 1.5f;

                case 7:
                    return 2;

                case 8:
                    return 4;

                case 9:
                    return 8;

                default:
                    throw new ArgumentException();
            }
        }
    }
}