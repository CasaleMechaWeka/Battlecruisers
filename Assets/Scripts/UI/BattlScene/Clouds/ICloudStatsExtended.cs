using UnityEngine;

namespace BattleCruisers.UI.BattleScene.Clouds
{
    // FELIX  Remove :)
    public interface ICloudStatsExtended
    {
        float HorizontalMovementSpeedInMPerS { get; }
        float DisappearLineInM { get; }
        float ReappaerLineInM { get; }
        Color FrontCloudColour { get; }
        Color BackCloudColour { get; }
    }
}
