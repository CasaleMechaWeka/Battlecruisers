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

        public void Initialise(Action navigationAction, IFilter shouldBeEnabledFilter)
        {
            base.Initialise();

            Helper.AssertIsNotNull(navigationAction, shouldBeEnabledFilter);

            _navigationAction = navigationAction;

            ButtonWrapper buttonWrapper = GetComponent<ButtonWrapper>();
            Assert.IsNotNull(buttonWrapper);
            buttonWrapper.Initialise(Handleclick, shouldBeEnabledFilter);
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
