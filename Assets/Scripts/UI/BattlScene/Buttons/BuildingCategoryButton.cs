using System;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.Utils;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.BattleScene.Buttons
{
    public class BuildingCategoryButton : UIElement, IBuildingCategoryButton, IFilter
	{
        private IUIManager _uiManager;
        private IFilter<BuildingCategory> _activenessDecider;

        public event EventHandler Clicked;

        public event EventHandler PotentialMatchChange
        {
            add { _activenessDecider.PotentialMatchChange += value; }
            remove { _activenessDecider.PotentialMatchChange -= value; }
        }

        public BuildingCategory Category { get; private set; }

        public bool IsMatch { get { return _activenessDecider.IsMatch(Category); } }

        public void Initialise(
            IBuildingGroup buildingGroup, 
            IUIManager uiManager, 
            IFilter<BuildingCategory> activenessDecider)
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
