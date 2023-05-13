using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using System.Collections.Generic;
using System.Linq;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Sorting
{
    public class PvPUnitUnlockedLevelSorter : PvPBuildableUnlockedLevelSorter, IPvPBuildableSorter<IPvPUnit>
    {
        public PvPUnitUnlockedLevelSorter(IStaticData staticData, IPvPBuildableKeyFactory keyFactory)
            : base(staticData, keyFactory) { }

        public IList<IPvPBuildableWrapper<IPvPUnit>> Sort(IList<IPvPBuildableWrapper<IPvPUnit>> units)
        {
            // return
            //     units
            //         .OrderBy(unit => _staticData.LevelFirstAvailableIn(_keyFactory.CreateUnitKey(unit.Buildable)))
            //         .ToList();

            return null;
        }
    }
}
