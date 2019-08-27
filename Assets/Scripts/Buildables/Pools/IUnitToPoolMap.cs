using BattleCruisers.Buildables.Units;
using BattleCruisers.Utils.BattleScene.Pools;

namespace BattleCruisers.Buildables.Pools
{
    public interface IUnitToPoolMap
    {
        IPool<Unit, BuildableActivationArgs> GetPool(IUnit unit);
    }
}