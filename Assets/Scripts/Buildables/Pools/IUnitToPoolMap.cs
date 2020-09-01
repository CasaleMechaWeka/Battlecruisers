using BattleCruisers.Buildables.Units;
using BattleCruisers.Utils.BattleScene.Pools;

namespace BattleCruisers.Buildables.Pools
{
    public interface IUnitToPoolMap
    {
        IPool<Unit, UnitActivationArgs> GetPool(IUnit unit);
    }
}