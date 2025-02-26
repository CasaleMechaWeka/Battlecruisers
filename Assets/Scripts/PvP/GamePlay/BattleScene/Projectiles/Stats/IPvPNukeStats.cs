using BattleCruisers.Projectiles.Stats;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Stats
{
    public interface IPvPNukeStats : ICruisingProjectileStats
    {
        Vector2 InitialVelocity { get; }
    }
}
