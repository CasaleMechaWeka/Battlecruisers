using UnityEngine;

namespace BattleCruisers.UI.BattleScene.Clouds.Stats
{
    // FELIX Test :)
    public class BackgroundImageCalculator : IBackgroundImageCalculator
    {
        public const float RATIO_4_TO_3 = 1.333f;
        public const float RATIO_16_TO_9 = 1.778f;

        public Vector3 FindPosition(IBackgroundImageStats stats, float cameraAspectRatio)
        {
            float deltaY = stats.YPositionAt16to9 - stats.PositionAt4to3.y;
            float deltaX = RATIO_16_TO_9 - RATIO_4_TO_3;
            float gradient = deltaY / deltaX;

            float constant = stats.YPositionAt16to9 - (gradient * RATIO_16_TO_9);

            float yAdjustedPosition = gradient * cameraAspectRatio + constant;

            return
                new Vector3(
                    stats.PositionAt4to3.x,
                    yAdjustedPosition,
                    stats.PositionAt4to3.z);
        }
    }
}