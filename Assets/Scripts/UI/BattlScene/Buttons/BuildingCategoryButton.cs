using System;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.BattleScene.Buttons
{
    public class BuildingCategoryButton : UIElement 
	{
        private BuildingCategory _buildingCategory;
        private IActivenessDecider<BuildingCategory> _activenessDecider;
        private ButtonWrapper _buttonWrapper;

        public void Initialise(
            IBuildingGroup buildingGroup, 
            IUIManager uiManager, 
            IActivenessDecider<BuildingCategory> activenessDecider)
		{
            base.Initialise();

            Helper.AssertIsNotNull(buildingGroup, uiManager, activenessDecider);

            _buildingCategory = buildingGroup.BuildingCategory;
            _activenessDecider = activenessDecider;
			_activenessDecider.PotentialActivenessChange += _activenessDecider_PotentialActivenessChange;

            _buttonWrapper = GetComponent<ButtonWrapper>();
            Assert.IsNotNull(_buttonWrapper);
            _buttonWrapper.Initialise();
            _buttonWrapper.Button.onClick.AddListener(() => uiManager.SelectBuildingGroup(buildingGroup.BuildingCategory));

            Text buttonText = _buttonWrapper.Button.GetComponentInChildren<Text>();
            Assert.IsNotNull(buttonText);
            buttonText.text = buildingGroup.BuildingGroupName;

            UpdateActiveness();
		}

        private void _activenessDecider_PotentialActivenessChange(object sender, EventArgs e)
        {
            UpdateActiveness();
        }

		private void UpdateActiveness()
		{
            _buttonWrapper.IsEnabled = _activenessDecider.ShouldBeEnabled(_buildingCategory);
		}
	}
}
