using BattleCruisers.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using BattleCruisers.Projectiles.Stats;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Spawners
{
    public interface IPvPProjectileSpawnerArgs
    {
        ITarget Parent { get; }
        ProjectileStats ProjectileStats { get; }
        int BurstSize { get; }
        PvPCruiserSpecificFactories CruiserSpecificFactories { get; }
        IPvPCruiser EnempCruiser { get; }
    }
}
