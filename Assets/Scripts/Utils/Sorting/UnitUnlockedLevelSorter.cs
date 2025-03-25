using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using System.Collections.Generic;
using System.Linq;

namespace BattleCruisers.Utils.Sorting
{
    public class UnitUnlockedLevelSorter : BuildableUnlockedLevelSorter, IBuildableSorter<IUnit>
    {
        public UnitUnlockedLevelSorter(IBuildableKeyFactory keyFactory)
            : base(keyFactory) { }

        public IList<IBuildableWrapper<IUnit>> Sort(IList<IBuildableWrapper<IUnit>> units)
        {
            return
                units
                    .OrderBy(unit => StaticData.UnitUnlockLevel(_keyFactory.CreateUnitKey(unit.Buildable)))
                    .ToList();
        }
    }
}
