using BattleCruisers.Buildables.Units;
using BattleCruisers.Utils.BattleScene.Pools;

namespace BattleCruisers.Buildables.Pools
{
    public interface IUnitToPoolMap
    {
        Pool<Unit, BuildableActivationArgs> GetPool(IUnit unit);
    }
}