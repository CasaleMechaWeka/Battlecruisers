using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene.Pools;
using BattleCruisers.Utils.Fetchers;

namespace BattleCruisers.Buildables.Pools
{
    public class UnitFactory : IPoolableFactory<Unit, BuildableActivationArgs>
    {
        private readonly IBuildableWrapper<IUnit> _unitPrefab;

        public UnitFactory(IPrefabKey unitKey)
        {
            Helper.AssertIsNotNull(unitKey);

            _unitPrefab = PrefabFactory.GetUnitWrapperPrefab(unitKey);
        }

        public Unit CreateItem()
        {
            return
                PrefabFactory
                    .CreateUnit(_unitPrefab)
                    .Parse<Unit>();
        }

        public override string ToString()
        {
            return $"{nameof(UnitFactory)} {_unitPrefab.Buildable}";
        }
    }
}