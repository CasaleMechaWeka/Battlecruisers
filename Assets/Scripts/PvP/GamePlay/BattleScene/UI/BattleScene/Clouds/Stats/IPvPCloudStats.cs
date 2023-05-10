using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Clouds.Stats
{
    public interface IPvPCloudStats
    {
        float HorizontalMovementSpeedInMPerS { get; }
        bool FlipClouds { get; }

        Color CloudColour { get; }
        float CloudYPosition { get; }
        float CloudZPosition { get; }

        Color MistColour { get; }
        float MistYPosition { get; }
        float MistZPosition { get; }

        Color FogColour { get; }
    }
}
