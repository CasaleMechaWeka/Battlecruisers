using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using System.Collections.Generic;
using System.Linq;

namespace BattleCruisers.Utils.Sorting
{
    // FELIX  Test, use
    public class UnitUnlockedLevelSorter : BuildableUnlockedLevelSorter, IBuildableSorter<IUnit>
    {
        public UnitUnlockedLevelSorter(IStaticData staticData, IBuildableKeyFactory keyFactory)
            : base(staticData, keyFactory) { }

        public IList<IBuildableWrapper<IUnit>> Sort(IList<IBuildableWrapper<IUnit>> units)
        {
            return
                units
                    .OrderBy(unit => _staticData.LevelFirstAvailableIn(_keyFactory.CreateUnitKey(unit.Buildable)))
                    .ToList();
        }
    }
}
