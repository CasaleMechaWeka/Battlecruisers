using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Scenes.Test.Utilities;
using UnityEngine;

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
            IBuildableWrapper<IUnit> unitWrapperPrefab = _prefabFactory.GetUnitWrapperPrefab(buildableKey);
            BuildableWrapper<IUnit> unitWrapper = Object.Instantiate(unitWrapperPrefab.UnityObject);
            unitWrapper.gameObject.SetActive(true);
            unitWrapper.Initialise();
            IUnit unit = unitWrapper.Buildable;
            _helper.InitialiseUnit(unit, args);
            return unit;
        }
    }
}
