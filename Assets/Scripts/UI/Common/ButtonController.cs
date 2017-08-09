using BattleCruisers.UI.Commands;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Common
{
    public class ButtonController : MonoBehaviour
    {
        private ICommand _command;

        public void Initialise(ICommand command)
        {
            Assert.IsNotNull(command);
			
            _command = command;
            _command.CanExecuteChanged += (sender, e) => UpdateVisibility();
        }

        public void Execute()
        {
            _command.Execute();
        }

        private void UpdateVisibility()
        {
            gameObject.SetActive(_command.CanExecute);
        }
    }
}
