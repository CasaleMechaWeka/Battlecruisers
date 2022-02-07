using BattleCruisers.Buildables.Units;
using BattleCruisers.Utils.BattleScene.Pools;

namespace BattleCruisers.Buildables.Pools
{
    public interface IUnitPoolProvider
    {
        // Aircraft
        IPool<Unit, BuildableActivationArgs> BomberPool { get; }
        IPool<Unit, BuildableActivationArgs> FighterPool { get; }
        IPool<Unit, BuildableActivationArgs> GunshipPool { get; }
        IPool<Unit, BuildableActivationArgs> TestAircraftPool { get; }

        // Ships
        IPool<Unit, BuildableActivationArgs> AttackBoatPool { get; }
        IPool<Unit, BuildableActivationArgs> AttackRIBPool { get; }
        IPool<Unit, BuildableActivationArgs> FrigatePool { get; }
        IPool<Unit, BuildableActivationArgs> DestroyerPool { get; }
        IPool<Unit, BuildableActivationArgs> ArchonPool { get; }
    }
}