using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Clouds.Stats
{
    public class PvPMoonStatsController : MonoBehaviour, IPvPMoonStats
    {
        public bool showMoon;
        public bool ShowMoon => showMoon;

        public RectTransform MoonTransform => (RectTransform)transform;

        public Color color;
        public Color Color => color;
    }
}