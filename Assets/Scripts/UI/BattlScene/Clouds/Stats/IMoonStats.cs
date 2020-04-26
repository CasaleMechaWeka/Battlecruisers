using UnityEngine;

namespace BattleCruisers.UI.BattleScene.Clouds.Stats
{
    public interface IMoonStats
    {
        Color Color { get; }
        RectTransform MoonTransform { get; }
        bool ShowMoon { get; }
    }
}