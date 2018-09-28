using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.UI.BattleScene.Buttons.ClickHandlers;
using BattleCruisers.UI.Filters;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.UI.BattleScene.Buttons
{
    public class BuildingButtonController : BuildableButtonController
	{
		private IBuildableWrapper<IBuilding> _buildingWrapper;
        private IBuildingClickHandler _clickHandler;

        public Image slotImage;

        public void Initialise(
            IBuildableWrapper<IBuilding> buildingWrapper, 
            IBuildingClickHandler clickHandler,
            IBroadcastingFilter<IBuildable> shouldBeEnabledFilter, 
            Sprite slotSprite)
		{
            base.Initialise(buildingWrapper.Buildable, shouldBeEnabledFilter);
			
			_buildingWrapper = buildingWrapper;
            _clickHandler = clickHandler;
			slotImage.sprite = slotSprite;
		}

        protected override void OnClicked(bool isButtonEnabled)
        {
            _clickHandler.HandleClick(isButtonEnabled, _buildingWrapper);
		}
	}
}
