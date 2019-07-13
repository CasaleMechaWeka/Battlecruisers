using BattleCruisers.Buildables;
using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Filters;
using BattleCruisers.Utils;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.Common.BuildableDetails.Buttons
{
    public class DeleteButtonController : ClickableTogglable
    {
        private IUIManager _uiManager;
        private IFilter<ITarget> _buttonVisibilityFilter;
        // Keep reference to avoid garbage collection
#pragma warning disable CS0414  // Variable is assigned but never used
        private FilterToggler _helpLabelsVisibilityToggler;
#pragma warning restore CS0414  // Variable is assigned but never used

        private Image _buttonImage;
        protected override MaskableGraphic Graphic => _buttonImage;

        private IBuildable _buildable;
        public IBuildable Buildable
        {
            private get { return _buildable; }
            set
            {
                _buildable = value;
                gameObject.SetActive(_buttonVisibilityFilter.IsMatch(_buildable));
            }
        }

        public void Initialise(
            IUIManager uiManager, 
            IFilter<ITarget> buttonVisibilityFilter,
            IBroadcastingFilter helpLabelVisibilityFilter)
        {
            base.Initialise();

            Helper.AssertIsNotNull(uiManager, buttonVisibilityFilter);

            _uiManager = uiManager;
            _buttonVisibilityFilter = buttonVisibilityFilter;

            _buttonImage = GetComponent<Image>();
            Assert.IsNotNull(_buttonImage);

            HelpLabel helpLabel = GetComponentInChildren<HelpLabel>();
            Assert.IsNotNull(helpLabel);
            helpLabel.Initialise();
            _helpLabelsVisibilityToggler = new FilterToggler(helpLabel, helpLabelVisibilityFilter);
        }

        protected override void OnClicked()
        {
            Buildable.InitiateDelete();
            _uiManager.HideItemDetails();
        }
    }
}
