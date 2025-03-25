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
        public UnitUnlockedLevelSorter(StaticData staticData, IBuildableKeyFactory keyFactory)
            : base(staticData, keyFactory) { }

        public IList<IBuildableWrapper<IUnit>> Sort(IList<IBuildableWrapper<IUnit>> units)
        {
            return
                units
                    .OrderBy(unit => _staticData.UnitUnlockLevel(_keyFactory.CreateUnitKey(unit.Buildable)))
                    .ToList();
        }
    }
}
