using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Cameras.Helpers;
using BattleCruisers.UI.Filters;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.UI.BattleScene.Buttons
{
    public class BuildingButtonController : BuildableButtonController
	{
		private IBuildableWrapper<IBuilding> _buildingWrapper;
        private IPlayerCruiserFocusHelper _playerCruiserFocusHelper;

        public Image slotImage;

        public void Initialise(
            IBuildableWrapper<IBuilding> buildingWrapper, 
            IUIManager uiManager, 
            IBroadcastingFilter<IBuildable> shouldBeEnabledFilter, 
            IPlayerCruiserFocusHelper playerCruiserFocusHelper,
            Sprite slotSprite)
		{
            base.Initialise(buildingWrapper.Buildable, uiManager, shouldBeEnabledFilter);
			
			_buildingWrapper = buildingWrapper;
            _playerCruiserFocusHelper = playerCruiserFocusHelper;
			slotImage.sprite = slotSprite;
		}

		protected override void HandleClick()
		{
            base.HandleClick();

            _playerCruiserFocusHelper.FocusOnPlayerCruiserIfNeeded();
			_uiManager.SelectBuildingFromMenu(_buildingWrapper);
		}
	}
}
