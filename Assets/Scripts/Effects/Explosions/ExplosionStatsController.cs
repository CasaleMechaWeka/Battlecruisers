using UnityEngine;

namespace BattleCruisers.Effects.Explosions
{
    public class ExplosionStatsController : MonoBehaviour, IExplosionStats
    {
        public ExplosionSize size;
        public ExplosionSize Size => size;

        public bool showTrails;
        public bool ShowTrails => showTrails;
    }
}