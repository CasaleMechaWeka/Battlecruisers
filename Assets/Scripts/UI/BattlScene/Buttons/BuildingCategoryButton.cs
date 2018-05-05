using System;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.Utils;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.BattleScene.Buttons
{
    public class BuildingCategoryButton : UIElement, IBuildingCategoryButton
	{
        private IActivenessDecider<BuildingCategory> _activenessDecider;
        private ButtonWrapper _buttonWrapper;
        private IUIManager _uiManager;

        public event EventHandler Clicked;

        public BuildingCategory Category { get; private set; }

        public void Initialise(
            IBuildingGroup buildingGroup, 
            IUIManager uiManager, 
            IActivenessDecider<BuildingCategory> activenessDecider)
		{
            base.Initialise();

            Helper.AssertIsNotNull(buildingGroup, uiManager, activenessDecider);

            _uiManager = uiManager;
            Category = buildingGroup.BuildingCategory;
            _activenessDecider = activenessDecider;
			_activenessDecider.PotentialActivenessChange += _activenessDecider_PotentialActivenessChange;

            _buttonWrapper = GetComponent<ButtonWrapper>();
            Assert.IsNotNull(_buttonWrapper);
            _buttonWrapper.Initialise(HandleClick);

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
            _buttonWrapper.IsEnabled = _activenessDecider.ShouldBeEnabled(Category);
		}

        private void HandleClick()
        {
            _uiManager.SelectBuildingGroup(Category);

            if (Clicked != null)
            {
                Clicked.Invoke(this, EventArgs.Empty);
            }
        }
	}
}
