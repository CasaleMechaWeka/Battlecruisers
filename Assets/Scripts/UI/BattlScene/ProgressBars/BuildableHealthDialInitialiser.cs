using BattleCruisers.Buildables;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.BattleScene.ProgressBars
{
    public class BuildableHealthDialInitialiser : HealthDialInitialiserBase<IBuildable>
    {
        protected override IFilter<IBuildable> CreateVisibilityFilter()
        {
            return new BuildableIsInitialisedFilter();
        }
    }
}