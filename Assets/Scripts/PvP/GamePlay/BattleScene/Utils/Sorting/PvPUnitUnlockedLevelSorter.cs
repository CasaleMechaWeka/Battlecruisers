using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Static;
using System.Collections.Generic;
using System.Linq;
using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Sorting
{
    public class PvPUnitUnlockedLevelSorter : IPvPBuildableSorter<IPvPUnit>
    {
        public PvPUnitUnlockedLevelSorter()
            : base() { }

        public IList<IPvPBuildableWrapper<IPvPUnit>> Sort(IList<IPvPBuildableWrapper<IPvPUnit>> units)
        {
            return
                units
                      //   .OrderBy(unit => StaticData.LevelFirstAvailableIn(_keyFactory.CreateUnitKey(unit.Buildable)))
                      .OrderBy(unit => StaticData.UnitUnlockLevel(new BattleCruisers.Data.Models.PrefabKeys.UnitKey(convertToPvP(unit.Buildable.Category), convertToPvP(unit.Buildable.PrefabName))))
                    .ToList();

            // return null;
        }

        private UnitCategory convertToPvP(UnitCategory category)
        {
            switch (category)
            {
                case UnitCategory.Aircraft:
                    return UnitCategory.Aircraft;
                case UnitCategory.Naval:
                    return UnitCategory.Naval;
                // case UnitCategory.Untouchable:
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
