using BattleCruisers.Buildables.Units;
using BattleCruisers.Utils.BattleScene.Pools;

namespace BattleCruisers.Buildables.Pools
{
    public interface IUnitPoolProvider
    {
        // Aircraft
        Pool<Unit, BuildableActivationArgs> BomberPool { get; }
        Pool<Unit, BuildableActivationArgs> FighterPool { get; }
        Pool<Unit, BuildableActivationArgs> GunshipPool { get; }
        Pool<Unit, BuildableActivationArgs> SteamCopterPool { get; }
        Pool<Unit, BuildableActivationArgs> BroadswordPool { get; }
        Pool<Unit, BuildableActivationArgs> StratBomberPool { get; }
        Pool<Unit, BuildableActivationArgs> SpyPlanePool { get; }
        Pool<Unit, BuildableActivationArgs> TestAircraftPool { get; }
        Pool<Unit, BuildableActivationArgs> MissileFighterPool { get; }

        // Ships
        Pool<Unit, BuildableActivationArgs> AttackBoatPool { get; }
        Pool<Unit, BuildableActivationArgs> AttackRIBPool { get; }
        Pool<Unit, BuildableActivationArgs> FrigatePool { get; }
        Pool<Unit, BuildableActivationArgs> DestroyerPool { get; }
        Pool<Unit, BuildableActivationArgs> SiegeDestroyerPool { get; }
        Pool<Unit, BuildableActivationArgs> ArchonPool { get; }
        Pool<Unit, BuildableActivationArgs> GlassCannoneerPool { get; }
        Pool<Unit, BuildableActivationArgs> GunBoatPool { get; }
        Pool<Unit, BuildableActivationArgs> RocketTurtlePool { get; }
        Pool<Unit, BuildableActivationArgs> FlakTurtlePool { get; }
    }
}