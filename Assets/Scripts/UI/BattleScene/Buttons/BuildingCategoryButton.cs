using BattleCruisers.Buildables.Buildings;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Filters;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using System;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.BattleScene.Buttons
{
    public class BuildingCategoryButton : CanvasGroupButton, IBuildingCategoryButton, IBroadcastingFilter
	{
        private IUIManager _uiManager;
        private IBroadcastingFilter<BuildingCategory> _shouldBeEnabledFilter;
        private FilterToggler _filterToggler, _helpLabelToggler;

        public Image activeFeedback;
        protected override MaskableGraphic Graphic => activeFeedback;

        public event EventHandler Clicked;

        public event EventHandler PotentialMatchChange
        {
            add { _shouldBeEnabledFilter.PotentialMatchChange += value; }
            remove { _shouldBeEnabledFilter.PotentialMatchChange -= value; }
        }

        public BuildingCategory category;
        public BuildingCategory Category => category;


        public bool IsMatch => _shouldBeEnabledFilter.IsMatch(Category);
        public bool IsActiveFeedbackVisible { set { activeFeedback.enabled = value; } }

        public void Initialise(
            ISingleSoundPlayer soundPlayer,
            BuildingCategory expectedBuildingCategory,
            IUIManager uiManager, 
            IBroadcastingFilter<BuildingCategory> shouldBeEnabledFilter,
            IBroadcastingFilter helpLabelEnabledFilter)
		{
            base.Initialise(soundPlayer);

            Helper.AssertIsNotNull(activeFeedback, uiManager, shouldBeEnabledFilter, helpLabelEnabledFilter);
            Assert.AreEqual(Category, expectedBuildingCategory);

            _uiManager = uiManager;
            _shouldBeEnabledFilter = shouldBeEnabledFilter;

            _filterToggler = new FilterToggler(this, this);

            Togglable helpLabel = transform.FindNamedComponent<Togglable>("HelpLabel");
            helpLabel.Initialise();
            _helpLabelToggler = new FilterToggler(helpLabelEnabledFilter, helpLabel);
		}

        private void OnDestroy()
        {
            Destroy(activeFeedback);
        }

        public void TriggerClick()
        {
            OnClicked();
        }

        protected override void OnClicked()
        {
            base.OnClicked();

            _uiManager.SelectBuildingGroup(Category);

            Clicked?.Invoke(this, EventArgs.Empty);
        }
    }
}
