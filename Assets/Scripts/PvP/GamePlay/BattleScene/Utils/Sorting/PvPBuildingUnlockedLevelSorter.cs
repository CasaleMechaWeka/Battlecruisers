using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using System.Collections.Generic;
using System.Linq;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Sorting
{
    public class PvPBuildingUnlockedLevelSorter : PvPBuildableUnlockedLevelSorter, IPvPBuildableSorter<IPvPBuilding>
    {
        public PvPBuildingUnlockedLevelSorter(IStaticData staticData, IPvPBuildableKeyFactory keyFactory)
            : base(staticData, keyFactory) { }

        public IList<IPvPBuildableWrapper<IPvPBuilding>> Sort(IList<IPvPBuildableWrapper<IPvPBuilding>> buildings)
        {
            // return
            //     buildings
            //         .OrderBy(building => _staticData.LevelFirstAvailableIn(_keyFactory.CreateBuildingKey(building.Buildable)))
            //         // So drone station comes before air and naval factories :P
            //         .ThenByDescending(building => building.Buildable.BuildTimeInS)
            //         .ToList();

            return null;
        }
    }
}
