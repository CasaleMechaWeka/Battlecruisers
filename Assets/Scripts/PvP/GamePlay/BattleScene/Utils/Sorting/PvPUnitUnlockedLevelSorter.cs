using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Static;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Sorting
{
    public class PvPUnitUnlockedLevelSorter : PvPBuildableUnlockedLevelSorter, IPvPBuildableSorter<IPvPUnit>
    {
        public PvPUnitUnlockedLevelSorter(IStaticData staticData, IPvPBuildableKeyFactory keyFactory)
            : base(staticData, keyFactory) { }

        public IList<IPvPBuildableWrapper<IPvPUnit>> Sort(IList<IPvPBuildableWrapper<IPvPUnit>> units)
        {
            return
                units
                      //   .OrderBy(unit => _staticData.LevelFirstAvailableIn(_keyFactory.CreateUnitKey(unit.Buildable)))
                      .OrderBy(unit => _staticData.LevelFirstAvailableIn(new BattleCruisers.Data.Models.PrefabKeys.UnitKey(convertToPvP(unit.Buildable.Category), convertToPvP(unit.Buildable.PrefabName))))
                    .ToList();

            // return null;
        }

        private BattleCruisers.Buildables.Units.UnitCategory convertToPvP(PvPUnitCategory category)
        {
            switch (category)
            {
                case PvPUnitCategory.Aircraft:
                    return UnitCategory.Aircraft;
                case PvPUnitCategory.Naval:
                    return UnitCategory.Naval;
                // case PvPUnitCategory.Untouchable:
                //     return UnitCategory.Untouchable;
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
