using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Models.PrefabKeys;

namespace BattleCruisers.Utils.Sorting
{
    public class BuildableSorterFactory : IBuildableSorterFactory
    {
        protected readonly BuildableKeyFactory _keyFactory;

        public BuildableSorterFactory(BuildableKeyFactory keyFactory)
        {
            Helper.AssertIsNotNull(keyFactory);

            _keyFactory = keyFactory;
        }

        public IBuildableSorter<IBuilding> CreateBuildingSorter()
        {
            return new BuildingUnlockedLevelSorter(_keyFactory);
        }

        public IBuildableSorter<IUnit> CreateUnitSorter()
        {
            return new UnitUnlockedLevelSorter(_keyFactory);
        }
    }
}
