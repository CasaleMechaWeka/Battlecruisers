using UnityEngine;

namespace BattleCruisers.Effects.Explosions
{
    // FELIX  Can use similar model to simplify ProjectileStatsWrapper?
    public class ExplosionStats : MonoBehaviour, IExplosionStats
    {
        public ExplosionSize size;
        public ExplosionSize Size { get { return size; } }

        public bool showTrails;
        public bool ShowTrails { get { return showTrails; } }
    }
}