using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Stats
{
    public class PvPNukeProjectileStats : PvPCruisingProjectileStats, IPvPNukeStats
    {
        public Vector2 InitialVelocity => new Vector2(0, 0);
    }
}