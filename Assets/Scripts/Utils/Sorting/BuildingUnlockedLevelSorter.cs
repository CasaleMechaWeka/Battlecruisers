using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using System.Collections.Generic;
using System.Linq;

namespace BattleCruisers.Utils.Sorting
{
    public class BuildingUnlockedLevelSorter : BuildableUnlockedLevelSorter, IBuildableSorter<IBuilding>
    {
        public BuildingUnlockedLevelSorter(IStaticData staticData, IBuildableKeyFactory keyFactory)
            : base(staticData, keyFactory) { }

        public IList<IBuildableWrapper<IBuilding>> Sort(IList<IBuildableWrapper<IBuilding>> buildings)
        {
            return
                buildings
                    .OrderBy(building => _staticData.LevelFirstAvailableIn(_keyFactory.CreateBuildingKey(building.Buildable)))
                    // So drone station comes before air and naval factories :P
                    .ThenByDescending(building => building.Buildable.BuildTimeInS)
                    .ToList();
        }
    }
}
