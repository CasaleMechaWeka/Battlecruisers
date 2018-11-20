using BattleCruisers.Buildables.Buildings;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Filters;
using BattleCruisers.Utils;
using System;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.BattleScene.Buttons
{
    // NEWUI  Replace old UI :P
    // NEWUI  Remove canvas groups if they are not being used (in inspector, with ButtonWrapper).
    //      Need separate sprites for this to work?
    public class BuildingCategoryButtonNEW : UIElement, IBuildingCategoryButton, IBroadcastingFilter
	{
        private IUIManager _uiManager;
        private IBroadcastingFilter<BuildingCategory> _shouldBeEnabledFilter;

        public Image activeFeedback;

        public event EventHandler Clicked;

        public event EventHandler PotentialMatchChange
        {
            add { _shouldBeEnabledFilter.PotentialMatchChange += value; }
            remove { _shouldBeEnabledFilter.PotentialMatchChange -= value; }
        }

        public BuildingCategory category;
        public BuildingCategory Category { get { return category; } }

        public bool IsMatch { get { return _shouldBeEnabledFilter.IsMatch(Category); } }
        public bool IsActiveFeedbackVisible { set { activeFeedback.enabled = value; } }

        public void Initialise(
            BuildingCategory expectedBuildingCategory,
            IUIManager uiManager, 
            IBroadcastingFilter<BuildingCategory> shouldBeEnabledFilter)
		{
            base.Initialise();

            Helper.AssertIsNotNull(activeFeedback, uiManager, shouldBeEnabledFilter);
            Assert.AreEqual(Category, expectedBuildingCategory);

            _uiManager = uiManager;
            _shouldBeEnabledFilter = shouldBeEnabledFilter;

            ButtonWrapper buttonWrapper = GetComponent<ButtonWrapper>();
            Assert.IsNotNull(buttonWrapper);
            buttonWrapper.Initialise(this, HandleClick);
		}

        private void HandleClick()
        {
            _uiManager.SelectBuildingGroup(Category);

            if (Clicked != null)
            {
                Clicked.Invoke(this, EventArgs.Empty);
            }
        }

        private void OnDestroy()
        {
            Destroy(activeFeedback);
        }
    }
}
