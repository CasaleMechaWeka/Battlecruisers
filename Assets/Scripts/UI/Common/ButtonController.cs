using BattleCruisers.UI.Commands;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.Common
{
    public class ButtonController : ClickableTogglable
    {
        private ICommand _command;

        protected override bool ToggleVisibility => true;

        private Image _buttonImage;
        protected override MaskableGraphic Graphic => _buttonImage;

        private CanvasGroup _canvasGroup;
        protected override CanvasGroup CanvasGroup => _canvasGroup;

        public void Initialise(ICommand command)
        {
            base.Initialise();

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
                _buttonImage = GetComponentInChildren<Image>();
                Assert.IsNotNull(_buttonImage);
            }
        }

        private void UpdateVisibility()
        {
            Enabled = _command.CanExecute;
        }

        protected override void OnClicked()
        {
            _command.Execute();
        }
    }
}
