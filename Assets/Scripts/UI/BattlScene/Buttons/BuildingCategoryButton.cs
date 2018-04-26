using System;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.BattleScene.Buttons
{
    public class BuildingCategoryButton : MonoBehaviour 
	{
        private BuildingCategory _buildingCategory;
        private IActivenessDecider<BuildingCategory> _activenessDecider;
        private CanvasGroup _canvasGroup;
		private Button _button;

        // FELIX  Avoid duplicate code with BuildableButtonController :P
        private bool IsEnabled
        {
            set
            {
                _button.enabled = value;
                _canvasGroup.alpha = value ? Constants.ENABLED_UI_ALPHA : Constants.DISABLED_UI_ALPHA;
            }
        }

        public void Initialise(
            IBuildingGroup buildingGroup, 
            IUIManager uiManager, 
            IActivenessDecider<BuildingCategory> activenessDecider)
		{
            Helper.AssertIsNotNull(buildingGroup, uiManager, activenessDecider);

            _buildingCategory = buildingGroup.BuildingCategory;
            _activenessDecider = activenessDecider;
			_activenessDecider.PotentialActivenessChange += _activenessDecider_PotentialActivenessChange;

            _canvasGroup = GetComponent<CanvasGroup>();
            Assert.IsNotNull(_canvasGroup);

            _button = GetComponent<Button>();
            Assert.IsNotNull(_button);
            _button.onClick.AddListener(() => uiManager.SelectBuildingGroup(buildingGroup.BuildingCategory));

            Text buttonText = _button.GetComponentInChildren<Text>();
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
            IsEnabled = _activenessDecider.ShouldBeEnabled(_buildingCategory);
		}
	}
}
