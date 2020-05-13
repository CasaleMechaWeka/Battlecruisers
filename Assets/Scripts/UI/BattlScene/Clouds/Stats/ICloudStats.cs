using UnityEngine;

namespace BattleCruisers.UI.BattleScene.Clouds.Stats
{
    public interface ICloudStats
    {
        float HorizontalMovementSpeedInMPerS { get; }
        bool FlipClouds { get; }
        
        Color CloudColour { get; }
        float CloudYPosition { get; }
        float CloudZPosition { get; }
        
        Color MistColour { get; }
        float MistYPosition { get; }
        float MistZPosition { get; }
    }
}
