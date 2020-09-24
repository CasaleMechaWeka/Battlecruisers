using UnityEngine;

namespace BattleCruisers.UI.BattleScene.Clouds.Stats
{
    public class MoonStatsController : MonoBehaviour, IMoonStats
    {
        public bool showMoon;
        public bool ShowMoon => showMoon;

        public RectTransform MoonTransform => (RectTransform)transform;

        public Color color;
        public Color Color => color;
    }
}