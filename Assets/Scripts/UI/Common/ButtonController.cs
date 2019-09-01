using BattleCruisers.UI.Commands;
using BattleCruisers.UI.Sound;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.Common
{
    public class ButtonController : ElementWithClickSound
    {
        private ICommand _command;

        protected override bool ToggleVisibility => true;

        protected Image _buttonImage;
        protected override MaskableGraphic Graphic => _buttonImage;

        private CanvasGroup _canvasGroup;
        protected override CanvasGroup CanvasGroup => _canvasGroup;

        public void Initialise(ISoundPlayer soundPlayer, ICommand command)
        {
            base.Initialise(soundPlayer);

            Assert.IsNotNull(command);
			
            _command = command;
            _command.CanExecuteChanged += (sender, e) => UpdateVisibility();

            GetAssets();

            UpdateVisibility();
        }

        protected virtual void GetAssets()
        {
            _canvasGroup = GetComponent<CanvasGroup>();

            if (_canvasGroup == null)
            {
                _buttonImage = GetComponentInChildren<Image>(includeInactive: true);
                Assert.IsNotNull(_buttonImage);
            }
        }

        private void UpdateVisibility()
        {
            Enabled = _command.CanExecute;
        }

        protected override void OnClicked()
        {
            base.OnClicked();
            _command.Execute();
        }
    }
}
