using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Utils.BattleScene.Pools;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Pools
{
    public interface IPvPUnitPoolProvider
    {
        // Aircraft
        Pool<PvPUnit, PvPBuildableActivationArgs> BomberPool { get; }
        Pool<PvPUnit, PvPBuildableActivationArgs> FighterPool { get; }
        Pool<PvPUnit, PvPBuildableActivationArgs> GunshipPool { get; }
        Pool<PvPUnit, PvPBuildableActivationArgs> SteamCopterPool { get; }
        Pool<PvPUnit, PvPBuildableActivationArgs> BroadswordPool { get; }
        Pool<PvPUnit, PvPBuildableActivationArgs> StratBomberPool { get; }
        Pool<PvPUnit, PvPBuildableActivationArgs> SpyPlanePool { get; }
        Pool<PvPUnit, PvPBuildableActivationArgs> MissileFighterPool { get; }
        Pool<PvPUnit, PvPBuildableActivationArgs> TestAircraftPool { get; }

        // Ships
        Pool<PvPUnit, PvPBuildableActivationArgs> AttackBoatPool { get; }
        Pool<PvPUnit, PvPBuildableActivationArgs> AttackRIBPool { get; }
        Pool<PvPUnit, PvPBuildableActivationArgs> FrigatePool { get; }
        Pool<PvPUnit, PvPBuildableActivationArgs> DestroyerPool { get; }
        Pool<PvPUnit, PvPBuildableActivationArgs> SiegeDestroyerPool { get; }
        Pool<PvPUnit, PvPBuildableActivationArgs> ArchonPool { get; }
        Pool<PvPUnit, PvPBuildableActivationArgs> GlassCannoneerPool { get; }
        Pool<PvPUnit, PvPBuildableActivationArgs> GunBoatPool { get; }
        Pool<PvPUnit, PvPBuildableActivationArgs> RocketTurtlePool { get; }
        Pool<PvPUnit, PvPBuildableActivationArgs> FlakTurtlePool { get; }
    }
}