using System;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.Utils;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.BattleScene.Buttons
{
    public class BuildingCategoryButton : UIElement, IBuildingCategoryButton, IActivenessDecider
	{
        private IUIManager _uiManager;
        private IActivenessDecider<BuildingCategory> _activenessDecider;

        public event EventHandler Clicked;

        public event EventHandler PotentialActivenessChange
        {
            add { _activenessDecider.PotentialActivenessChange += value; }
            remove { _activenessDecider.PotentialActivenessChange -= value; }
        }

        public BuildingCategory Category { get; private set; }

        public bool ShouldBeEnabled { get { return _activenessDecider.ShouldBeEnabled(Category); } }

        public void Initialise(
            IBuildingGroup buildingGroup, 
            IUIManager uiManager, 
            IActivenessDecider<BuildingCategory> activenessDecider)
		{
            base.Initialise();

            Helper.AssertIsNotNull(buildingGroup, uiManager, activenessDecider);

            _uiManager = uiManager;
            _activenessDecider = activenessDecider;
            Category = buildingGroup.BuildingCategory;

            ButtonWrapper buttonWrapper = GetComponent<ButtonWrapper>();
            Assert.IsNotNull(buttonWrapper);
            buttonWrapper.Initialise(HandleClick, this);

            Text buttonText = buttonWrapper.Button.GetComponentInChildren<Text>();
            Assert.IsNotNull(buttonText);
            buttonText.text = buildingGroup.BuildingGroupName;
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
