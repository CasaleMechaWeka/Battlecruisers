using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Data.Static;
using System.Collections.Generic;
using System.Linq;
using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Sorting
{
    public class PvPBuildingUnlockedLevelSorter : PvPBuildableUnlockedLevelSorter, IPvPBuildableSorter<IPvPBuilding>
    {
        public PvPBuildingUnlockedLevelSorter()
            : base() { }

        public IList<IPvPBuildableWrapper<IPvPBuilding>> Sort(IList<IPvPBuildableWrapper<IPvPBuilding>> buildings)
        {
            return
                buildings
                    // .OrderBy(building => StaticData.LevelFirstAvailableIn(_keyFactory.CreateBuildingKey(building.Buildable)))
                    .OrderBy(building => StaticData.BuildingUnlockLevel(new BattleCruisers.Data.Models.PrefabKeys.BuildingKey(convertToPvP(building.Buildable.Category), convertToPvP(building.Buildable.PrefabName))))
                    // So drone station comes before air and naval factories :P
                    .ThenByDescending(building => building.Buildable.BuildTimeInS)
                    .ToList();

            // return null;
        }


        private BuildingCategory convertToPvP(BuildingCategory category)
        {
            switch (category)
            {
                case BuildingCategory.Defence:
                    return BuildingCategory.Defence;
                case BuildingCategory.Factory:
                    return BuildingCategory.Factory;
                case BuildingCategory.Offence:
                    return BuildingCategory.Offence;
                case BuildingCategory.Tactical:
                    return BuildingCategory.Tactical;
                case BuildingCategory.Ultra:
                    return BuildingCategory.Ultra;
                default:
                    throw new NullReferenceException();
            }
        }

        private string convertToPvP(string key)
        {
            return key.Replace("PvP", "");
        }
    }
}
