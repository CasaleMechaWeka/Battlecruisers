using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;

namespace BattleCruisers.Utils.Sorting
{
    // FELIX  Test, use
    public abstract class BuildableUnlockedLevelSorter
    {
        protected readonly IStaticData _staticData;
        protected readonly IBuildableKeyFactory _keyFactory;

        protected BuildableUnlockedLevelSorter(IStaticData staticData, IBuildableKeyFactory keyFactory)
        {
            Helper.AssertIsNotNull(staticData, keyFactory);

            _staticData = staticData;
            _keyFactory = keyFactory;
        }
    }
}