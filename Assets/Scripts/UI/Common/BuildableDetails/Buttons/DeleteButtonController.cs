using BattleCruisers.Buildables;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.Common.BuildableDetails.Buttons
{
    public class DeleteButtonController : CanvasGroupButton
    {
        private IUIManager _uiManager;
        private IFilter<ITarget> _buttonVisibilityFilter;

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
            ISingleSoundPlayer soundPlayer,
            IUIManager uiManager, 
            IFilter<ITarget> buttonVisibilityFilter,
            IDismissableEmitter parent)
        {
            base.Initialise(soundPlayer, parent: parent);

            Helper.AssertIsNotNull(uiManager, buttonVisibilityFilter);

            _uiManager = uiManager;
            _buttonVisibilityFilter = buttonVisibilityFilter;

            _buttonImage = GetComponent<Image>();
            Assert.IsNotNull(_buttonImage);
        }

        protected override void OnClicked()
        {
            base.OnClicked();

            Buildable.InitiateDelete();
            _uiManager.HideItemDetails();
        }
    }
}
