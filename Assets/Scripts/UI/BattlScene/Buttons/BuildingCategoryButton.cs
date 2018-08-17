using BattleCruisers.Buildables.Buildings;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Filters;
using BattleCruisers.Utils;
using System;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.BattleScene.Buttons
{
    public class BuildingCategoryButton : UIElement, IBuildingCategoryButton, IBroadcastingFilter
	{
        private IUIManager _uiManager;
        private IBroadcastingFilter<BuildingCategory> _shouldBeEnabledFilter;

        public event EventHandler Clicked;

        public event EventHandler PotentialMatchChange
        {
            add { _shouldBeEnabledFilter.PotentialMatchChange += value; }
            remove { _shouldBeEnabledFilter.PotentialMatchChange -= value; }
        }

        public BuildingCategory Category { get; private set; }

        public bool IsMatch { get { return _shouldBeEnabledFilter.IsMatch(Category); } }

        public void Initialise(
            IBuildingGroup buildingGroup, 
            IUIManager uiManager, 
            IBroadcastingFilter<BuildingCategory> shouldBeEnabledFilter)
		{
            base.Initialise();

            Helper.AssertIsNotNull(buildingGroup, uiManager, shouldBeEnabledFilter);

            _uiManager = uiManager;
            _shouldBeEnabledFilter = shouldBeEnabledFilter;
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
