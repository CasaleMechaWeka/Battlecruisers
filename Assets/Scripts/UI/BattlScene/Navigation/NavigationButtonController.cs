using System;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.BattleScene.Navigation
{
    public class NavigationButtonController : UIElement, IButton
    {
        private Action _navigationAction;

        public event EventHandler Clicked;

        public void Initialise(Action navigationAction)
        {
            base.Initialise();

            Assert.IsNotNull(navigationAction);
            _navigationAction = navigationAction;

            Button button = GetComponent<Button>();
            Assert.IsNotNull(button);
            button.onClick.AddListener(ClickHandler);
        }

        private void ClickHandler()
        {
            _navigationAction.Invoke();

            if (Clicked != null)
            {
                Clicked.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
