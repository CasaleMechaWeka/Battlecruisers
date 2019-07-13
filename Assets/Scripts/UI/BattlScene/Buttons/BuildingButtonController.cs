using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.UI.BattleScene.Buttons.ClickHandlers;
using BattleCruisers.UI.Filters;

namespace BattleCruisers.UI.BattleScene.Buttons
{
    public class BuildingButtonController : BuildableButtonController
	{
		private IBuildableWrapper<IBuilding> _buildingWrapper;
        private IBuildingClickHandler _clickHandler;

        public void Initialise(
            IBuildableWrapper<IBuilding> buildingWrapper, 
            IBuildingClickHandler clickHandler,
            IBroadcastingFilter<IBuildable> shouldBeEnabledFilter)
		{
            base.Initialise(buildingWrapper.Buildable, shouldBeEnabledFilter);
			
			_buildingWrapper = buildingWrapper;
            _clickHandler = clickHandler;
		}

        protected override void HandleClick(bool isButtonEnabled)
        {
            _clickHandler.HandleClick(isButtonEnabled, _buildingWrapper);
		}
	}
}
