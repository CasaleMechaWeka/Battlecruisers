using BattleCruisers.Data.Models;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Helpers
{
    public class PvPZoomLevelConverter : IPvPLevelToMultiplierConverter
    {
        public float LevelToMultiplier(int zoomLevel)
        {
            Assert.IsTrue(zoomLevel >= SettingsModel.MIN_ZOOM_SPEED_LEVEL);
            Assert.IsTrue(zoomLevel <= SettingsModel.MAX_ZOOM_SPEED_LEVEL);

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