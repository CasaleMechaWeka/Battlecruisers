using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Stats
{
    public interface IPvPNukeStats : IPvPCruisingProjectileStats
    {
        Vector2 InitialVelocity { get; }
    }
}
