using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using System.Collections.Generic;
using System.Linq;
using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Sorting
{
    public class PvPBuildingUnlockedLevelSorter : PvPBuildableUnlockedLevelSorter, IPvPBuildableSorter<IPvPBuilding>
    {
        public PvPBuildingUnlockedLevelSorter(IStaticData staticData, IPvPBuildableKeyFactory keyFactory)
            : base(staticData, keyFactory) { }

        public IList<IPvPBuildableWrapper<IPvPBuilding>> Sort(IList<IPvPBuildableWrapper<IPvPBuilding>> buildings)
        {
            return
                buildings
                    // .OrderBy(building => _staticData.LevelFirstAvailableIn(_keyFactory.CreateBuildingKey(building.Buildable)))
                    .OrderBy(building => _staticData.LevelFirstAvailableIn(new BattleCruisers.Data.Models.PrefabKeys.BuildingKey(convertToPvP(building.Buildable.Category), convertToPvP(building.Buildable.PrefabName))))
                    // So drone station comes before air and naval factories :P
                    .ThenByDescending(building => building.Buildable.BuildTimeInS)
                    .ToList();

            // return null;
        }


        private BattleCruisers.Buildables.Buildings.BuildingCategory convertToPvP(PvPBuildingCategory category)
        {
            switch (category)
            {
                case PvPBuildingCategory.Defence:
                    return BattleCruisers.Buildables.Buildings.BuildingCategory.Defence;
                case PvPBuildingCategory.Factory:
                    return BattleCruisers.Buildables.Buildings.BuildingCategory.Factory;
                case PvPBuildingCategory.Offence:
                    return BattleCruisers.Buildables.Buildings.BuildingCategory.Offence;
                case PvPBuildingCategory.Tactical:
                    return BattleCruisers.Buildables.Buildings.BuildingCategory.Tactical;
                case PvPBuildingCategory.Ultra:
                    return BattleCruisers.Buildables.Buildings.BuildingCategory.Ultra;
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
