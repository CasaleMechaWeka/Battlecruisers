using UnityEngine;

namespace BattleCruisers.UI.BattleScene.Clouds
{
    public interface ICloudStats
    {
        float HorizontalMovementSpeedInMPerS { get; }
        float DisappearLineInM { get; }
        float ReappaerLineInM { get; }
        Color FrontCloudColour { get; }
        Color BackCloudColour { get; }
    }
}
