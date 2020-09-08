using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;

namespace BattleCruisers.Utils.Sorting
{
    public class BuildableSorterFactory : IBuildableSorterFactory
    {
        protected readonly IStaticData _staticData;
        protected readonly IBuildableKeyFactory _keyFactory;

        public BuildableSorterFactory(IStaticData staticData, IBuildableKeyFactory keyFactory)
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
