using System;
using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.Navigation
{
    public class NavigationButtonController : UIElement, IButton
    {
        private Action _navigationAction;

        public event EventHandler Clicked;

        public void Initialise(Action navigationAction, IActivenessDecider activenessDecider)
        {
            base.Initialise();

            Helper.AssertIsNotNull(navigationAction, activenessDecider);

            _navigationAction = navigationAction;

            ButtonWrapper buttonWrapper = GetComponent<ButtonWrapper>();
            Assert.IsNotNull(buttonWrapper);
            buttonWrapper.Initialise(Handleclick, activenessDecider);
        }

        private void Handleclick()
        {
            _navigationAction.Invoke();

            if (Clicked != null)
            {
                Clicked.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
