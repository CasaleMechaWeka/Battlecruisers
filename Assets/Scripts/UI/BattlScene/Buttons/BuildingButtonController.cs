using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.UI.BattleScene.Manager;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.UI.BattleScene.Buttons
{
    public class BuildingButtonController : BuildableButtonController
	{
		private IBuildableWrapper<IBuilding> _buildingWrapper;

		public Image slotImage;

        public void Initialise(IBuildableWrapper<IBuilding> buildingWrapper, IUIManager uiManager, IBuildableButtonActivenessDecider activenessDecider, Sprite slotSprite)
		{
            base.Initialise(buildingWrapper.Buildable, uiManager, activenessDecider);
			
			_buildingWrapper = buildingWrapper;
			slotImage.sprite = slotSprite;
		}

		protected override void OnClick()
		{
			_uiManager.SelectBuildingFromMenu(_buildingWrapper);
		}
	}
}
