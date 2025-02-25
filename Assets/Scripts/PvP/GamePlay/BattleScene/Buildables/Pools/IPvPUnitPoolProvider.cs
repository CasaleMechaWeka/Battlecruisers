using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Utils.BattleScene.Pools;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Pools
{
    public interface IPvPUnitPoolProvider
    {
        // Aircraft
        IPool<PvPUnit, PvPBuildableActivationArgs> BomberPool { get; }
        IPool<PvPUnit, PvPBuildableActivationArgs> FighterPool { get; }
        IPool<PvPUnit, PvPBuildableActivationArgs> GunshipPool { get; }
        IPool<PvPUnit, PvPBuildableActivationArgs> SteamCopterPool { get; }
        IPool<PvPUnit, PvPBuildableActivationArgs> BroadswordPool { get; }
        IPool<PvPUnit, PvPBuildableActivationArgs> StratBomberPool { get; }
        IPool<PvPUnit, PvPBuildableActivationArgs> SpyPlanePool { get; }
        IPool<PvPUnit, PvPBuildableActivationArgs> MissileFighterPool { get; }
        IPool<PvPUnit, PvPBuildableActivationArgs> TestAircraftPool { get; }

        // Ships
        IPool<PvPUnit, PvPBuildableActivationArgs> AttackBoatPool { get; }
        IPool<PvPUnit, PvPBuildableActivationArgs> AttackRIBPool { get; }
        IPool<PvPUnit, PvPBuildableActivationArgs> FrigatePool { get; }
        IPool<PvPUnit, PvPBuildableActivationArgs> DestroyerPool { get; }
        IPool<PvPUnit, PvPBuildableActivationArgs> SiegeDestroyerPool { get; }
        IPool<PvPUnit, PvPBuildableActivationArgs> ArchonPool { get; }
        IPool<PvPUnit, PvPBuildableActivationArgs> GlassCannoneerPool { get; }
        IPool<PvPUnit, PvPBuildableActivationArgs> GunBoatPool { get; }
        IPool<PvPUnit, PvPBuildableActivationArgs> RocketTurtlePool { get; }
        IPool<PvPUnit, PvPBuildableActivationArgs> FlakTurtlePool { get; }
    }
}