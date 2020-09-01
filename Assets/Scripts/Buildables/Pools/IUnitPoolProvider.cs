using BattleCruisers.Buildables.Units;
using BattleCruisers.Utils.BattleScene.Pools;

namespace BattleCruisers.Buildables.Pools
{
    public interface IUnitPoolProvider
    {
        // Aircraft
        IPool<Unit, UnitActivationArgs> BomberPool { get; }
        IPool<Unit, UnitActivationArgs> FighterPool { get; }
        IPool<Unit, UnitActivationArgs> GunshipPool { get; }
        IPool<Unit, UnitActivationArgs> TestAircraftPool { get; }

        // Ships
        IPool<Unit, UnitActivationArgs> AttackBoatPool { get; }
        IPool<Unit, UnitActivationArgs> FrigatePool { get; }
        IPool<Unit, UnitActivationArgs> DestroyerPool { get; }
        IPool<Unit, UnitActivationArgs> ArchonPool { get; }
    }
}