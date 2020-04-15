using UnityEngine;

namespace BattleCruisers.UI.BattleScene.Clouds.Stats
{
    public enum CloudMovementSpeed
    {
        Slow, Fast
    }

    public interface ICloudStats
    {
        float Height { get; }
        float HorizontalMovementSpeedInMPerS { get; }
        bool FlipClouds { get; }
        Color CloudColour { get; }
        Color MistColour { get; }
    }
}
