using BattleCruisers.Data.Models.PrefabKeys;

namespace BattleCruisers.Utils.Sorting
{
    public abstract class BuildableUnlockedLevelSorter
    {
        protected readonly IBuildableKeyFactory _keyFactory;

        protected BuildableUnlockedLevelSorter(IBuildableKeyFactory keyFactory)
        {
            Helper.AssertIsNotNull(keyFactory);

            _keyFactory = keyFactory;
        }
    }
}