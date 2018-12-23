using BattleCruisers.Buildables.Buildings;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Filters;
using BattleCruisers.Utils;
using System;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BattleCruisers.UI.BattleScene.Buttons
{
    public class BuildingCategoryButton : UIElement, 
        IBuildingCategoryButton, 
        IBroadcastingFilter, 
        IPointerClickHandler
	{
        private IUIManager _uiManager;
        private IBroadcastingFilter<BuildingCategory> _shouldBeEnabledFilter;
        private FilterToggler _filterToggler, _helpLabelToggler;

        public Image activeFeedback;

        public event EventHandler Clicked;

        public event EventHandler PotentialMatchChange
        {
            add { _shouldBeEnabledFilter.PotentialMatchChange += value; }
            remove { _shouldBeEnabledFilter.PotentialMatchChange -= value; }
        }

        public BuildingCategory category;
        public BuildingCategory Category { get { return category; } }

        private Image _buttonImage;
        protected override Image Image { get { return _buttonImage; } }

        public bool IsMatch { get { return _shouldBeEnabledFilter.IsMatch(Category); } }
        public bool IsActiveFeedbackVisible { set { activeFeedback.enabled = value; } }

        public void Initialise(
            BuildingCategory expectedBuildingCategory,
            IUIManager uiManager, 
            IBroadcastingFilter<BuildingCategory> shouldBeEnabledFilter,
            IBroadcastingFilter helpLabelEnabledFilter)
		{
            base.Initialise();

            Helper.AssertIsNotNull(activeFeedback, uiManager, shouldBeEnabledFilter, helpLabelEnabledFilter);
            Assert.AreEqual(Category, expectedBuildingCategory);

            _uiManager = uiManager;
            _shouldBeEnabledFilter = shouldBeEnabledFilter;

            _buttonImage = GetComponent<Image>();
            Assert.IsNotNull(_buttonImage);

            _filterToggler = new FilterToggler(this, this);

            Togglable helpLabel = transform.FindNamedComponent<Togglable>("HelpLabel");
            helpLabel.Initialise();
            _helpLabelToggler = new FilterToggler(helpLabel, helpLabelEnabledFilter);
		}

        private void OnDestroy()
        {
            Destroy(activeFeedback);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _uiManager.SelectBuildingGroup(Category);

            if (Clicked != null)
            {
                Clicked.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
