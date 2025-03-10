using BattleCruisers.Projectiles.Stats;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Stats
{
    public class PvPNukeProjectileStats : PvPCruisingProjectileStats, INukeStats
    {
        public Vector2 InitialVelocity => new Vector2(0, 0);
    }
}