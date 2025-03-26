using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using System.Collections.Generic;
using System.Linq;

namespace BattleCruisers.Utils.Sorting
{
    public class BuildingUnlockedLevelSorter : IBuildableSorter<IBuilding>
    {
        public BuildingUnlockedLevelSorter()
            : base() { }

        public IList<IBuildableWrapper<IBuilding>> Sort(IList<IBuildableWrapper<IBuilding>> buildings)
        {
            return
                buildings
                    .OrderBy(building => StaticData.BuildingUnlockLevel(CreateBuildingKey(building.Buildable)))
                    // So drone station comes before air and naval factories :P
                    .ThenByDescending(building => building.Buildable.BuildTimeInS)
                    .ToList();
        }

        private BuildingKey CreateBuildingKey(IBuilding building)
        {
            return new BuildingKey(building.Category, building.PrefabName);
        }
    }
}
