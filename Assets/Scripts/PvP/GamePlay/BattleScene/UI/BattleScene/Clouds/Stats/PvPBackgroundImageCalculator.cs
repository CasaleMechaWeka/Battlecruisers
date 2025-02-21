using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Clouds.Stats
{
    public class PvPBackgroundImageCalculator : IPvPBackgroundImageCalculator
    {
        public const float RATIO_4_TO_3 = 1.333f;
        public const float RATIO_16_TO_9 = 1.778f;
        public const float RATIO_24_TO_10 = 2.4f;

        public Vector3 FindPosition(IPvPBackgroundImageStats stats, float cameraAspectRatio)
        {
            float deltaY = stats.YPositionAt16to9 - stats.PositionAt4to3.y;
            float deltaX = RATIO_16_TO_9 - RATIO_4_TO_3;
            float gradient = deltaY / deltaX;

            float constant = stats.YPositionAt16to9 - (gradient * RATIO_16_TO_9);
            if (cameraAspectRatio > RATIO_16_TO_9)
            {
                constant -= 70 * cameraAspectRatio;
            }
            float yAdjustedPosition = gradient * cameraAspectRatio + constant;

            float deltaY_16to9_to_24to10 = stats.YPositionAt24to10 - stats.YPositionAt16to9;
            float deltaX_16to9_to_24to10 = RATIO_24_TO_10 - RATIO_16_TO_9;
            float gradient_16to9_to_24to10 = deltaY_16to9_to_24to10 / deltaX_16to9_to_24to10;

            if (cameraAspectRatio > RATIO_16_TO_9 && cameraAspectRatio <= RATIO_24_TO_10) {
                float constant_16to9_to_24to10 = stats.YPositionAt16to9 - (gradient_16to9_to_24to10 * RATIO_16_TO_9);
                float yAdjustedPosition_16to9_to_24to10 = gradient_16to9_to_24to10 * cameraAspectRatio + constant_16to9_to_24to10;
                return new Vector3(
                    stats.PositionAt4to3.x,
                    yAdjustedPosition_16to9_to_24to10,
                    stats.PositionAt4to3.z
                );
            }

            return
                new Vector3(
                    stats.PositionAt4to3.x,
                    yAdjustedPosition,
                    stats.PositionAt4to3.z);
        }
    }
}