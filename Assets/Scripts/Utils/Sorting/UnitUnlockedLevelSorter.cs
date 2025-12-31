using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using System.Collections.Generic;
using System.Linq;

namespace BattleCruisers.Utils.Sorting
{
    public class UnitUnlockedLevelSorter : IBuildableSorter<IUnit>
    {
        public UnitUnlockedLevelSorter()
            : base() { }

        public IList<IBuildableWrapper<IUnit>> Sort(IList<IBuildableWrapper<IUnit>> units)
        {
            return
                units
                    .OrderBy(unit => StaticData.UnitUnlockLevel(CreateUnitKey(unit.Buildable)))
                    .ToList();
        }

        private UnitKey CreateUnitKey(IUnit unit)
        {
            return new UnitKey(unit.Category, unit.PrefabName);
        }
    }
}
