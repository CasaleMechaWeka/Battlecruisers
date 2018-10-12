using UnityEngine;

namespace BattleCruisers.Effects.Explosions
{
    public class ExplosionStatsController : MonoBehaviour, IExplosionStats
    {
        public ExplosionSize size;
        public ExplosionSize Size { get { return size; } }

        public bool showTrails;
        public bool ShowTrails { get { return showTrails; } }
    }
}