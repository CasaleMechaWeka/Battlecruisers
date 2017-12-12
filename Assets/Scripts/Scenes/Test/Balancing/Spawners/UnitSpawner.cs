using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Fetchers;
using BattleCruisers.Scenes.Test.Utilities;

namespace BattleCruisers.Scenes.Test.Balancing.Spawners
{
    public class UnitSpawner : BuildableSpawner
    {
        public UnitSpawner(IPrefabFactory prefabFactory, Helper helper)
            : base(prefabFactory, helper)
        {
        }

        protected override IBuildable SpawnBuildable(IPrefabKey buildableKey, BuildableInitialisationArgs args)
        {
            IBuildableWrapper<IUnit> unitWrapper = _prefabFactory.GetUnitWrapperPrefab(buildableKey);
            IUnit unit = _prefabFactory.CreateUnit(unitWrapper);
            _helper.InitialiseUnit(unit, args);
            return unit;
        }
    }
}
