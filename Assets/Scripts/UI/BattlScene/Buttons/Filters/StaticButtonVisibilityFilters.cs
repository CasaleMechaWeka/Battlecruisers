using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.UI.Filters;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.BattleScene.Buttons.Filters
{
    // NEWUI  Remove is unused after using new UI?
    public class StaticButtonVisibilityFilters : IButtonVisibilityFilters
    {
        public IBroadcastingFilter<IBuildable> BuildableButtonVisibilityFilter { get; private set; }
        public IBroadcastingFilter<BuildingCategory> CategoryButtonVisibilityFilter { get; private set; }
        public IFilter<ITarget> ChooseTargetButtonVisiblityFilter { get; private set; }
        public IFilter<ITarget> DeletButtonVisiblityFilter { get; private set; }
        public BasicFilter BackButtonVisibilityFilter { get; private set; }

        public StaticButtonVisibilityFilters(bool isMatch)
        {
            BuildableButtonVisibilityFilter = new StaticBroadcastingFilter<IBuildable>(isMatch);
            CategoryButtonVisibilityFilter = new StaticBroadcastingFilter<BuildingCategory>(isMatch);
            ChooseTargetButtonVisiblityFilter = new StaticFilter<ITarget>(isMatch);
            DeletButtonVisiblityFilter = new StaticFilter<ITarget>(isMatch);
            BackButtonVisibilityFilter = new BasicFilter(isMatch);
        }
    }
}