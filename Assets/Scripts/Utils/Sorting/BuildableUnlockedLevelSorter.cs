using BattleCruisers.Data.Models.PrefabKeys;

namespace BattleCruisers.Utils.Sorting
{
    public abstract class BuildableUnlockedLevelSorter
    {
        protected readonly BuildableKeyFactory _keyFactory;

        protected BuildableUnlockedLevelSorter(BuildableKeyFactory keyFactory)
        {
            Helper.AssertIsNotNull(keyFactory);

            _keyFactory = keyFactory;
        }
    }
}