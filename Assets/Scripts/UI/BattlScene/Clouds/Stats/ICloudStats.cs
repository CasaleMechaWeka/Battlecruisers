using UnityEngine;

namespace BattleCruisers.UI.BattleScene.Clouds.Stats
{
    public interface ICloudStats
    {
        float Height { get; }
        float HorizontalMovementSpeedInMPerS { get; }
        bool FlipClouds { get; }
        Color CloudColour { get; }
        Color MistColour { get; }
    }
}
