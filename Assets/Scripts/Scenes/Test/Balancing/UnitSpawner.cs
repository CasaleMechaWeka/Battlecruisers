using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Fetchers;
using TestUtils = BattleCruisers.Scenes.Test.Utilities;

namespace BattleCruisers.Scenes.Test.Balancing
{
    public class UnitSpawner : BuildableSpawner
    {
        public UnitSpawner(IPrefabFactory prefabFactory, TestUtils.Helper helper)
            : base(prefabFactory, helper)
        {
        }

        protected override IBuildable SpawnBuildable(IPrefabKey buildableKey, Faction faction, Direction facingDirection)
        {
            IBuildableWrapper<IUnit> unitWrapper = _prefabFactory.GetUnitWrapperPrefab(buildableKey);
            IUnit unit = _prefabFactory.CreateUnit(unitWrapper);
            _helper.InitialiseUnit(unit, faction, parentCruiserDirection: facingDirection);
            return unit;
        }
    }
}
