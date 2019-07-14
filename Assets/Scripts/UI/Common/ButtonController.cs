using BattleCruisers.UI.Commands;
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

        public void Initialise(ICommand command)
        {
            Assert.IsNotNull(command);
			
            _command = command;
            _command.CanExecuteChanged += (sender, e) => UpdateVisibility();

            _buttonImage = GetComponentInChildren<Image>();
            Assert.IsNotNull(_buttonImage);

            UpdateVisibility();
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
