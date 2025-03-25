using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;

namespace BattleCruisers.Utils.Sorting
{
    public class BuildableSorterFactory : IBuildableSorterFactory
    {
        protected readonly StaticData _staticData;
        protected readonly IBuildableKeyFactory _keyFactory;

        public BuildableSorterFactory(StaticData staticData, IBuildableKeyFactory keyFactory)
        {
            Helper.AssertIsNotNull(staticData, keyFactory);

            _staticData = staticData;
            _keyFactory = keyFactory;
        }

        public IBuildableSorter<IBuilding> CreateBuildingSorter()
        {
            return new BuildingUnlockedLevelSorter(_staticData, _keyFactory);
        }

        public IBuildableSorter<IUnit> CreateUnitSorter()
        {
            return new UnitUnlockedLevelSorter(_staticData, _keyFactory);
        }
    }
}
