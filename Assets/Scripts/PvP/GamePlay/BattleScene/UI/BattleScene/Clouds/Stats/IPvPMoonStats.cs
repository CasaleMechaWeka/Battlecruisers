using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Clouds.Stats
{
    public interface IPvPMoonStats
    {
        Color Color { get; }
        RectTransform MoonTransform { get; }
        bool ShowMoon { get; }
    }
}