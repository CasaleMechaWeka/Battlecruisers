using UnityEngine;

namespace BattleCruisers.UI.BattleScene.Clouds.Stats
{
    public interface ICloudStats
    {
        float HorizontalMovementSpeedInMPerS { get; }
        bool FlipClouds { get; }
        
        Color CloudColour { get; }
        float Height { get; }
        float CloudZPosition { get; }
        
        Color MistColour { get; }
        float MistYPosition { get; }
        float MistZPosiiton { get; }
    }
}
