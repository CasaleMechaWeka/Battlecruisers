using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.UI.Filters;
using BattleCruisers.Utils;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.Navigation
{
    public class NavigationButtonController : UIElement, IButton
    {
        private Action _navigationAction;

        public event EventHandler Clicked;

        public void Initialise(Action navigationAction, IBroadcastingFilter shouldBeEnabledFilter)
        {
            base.Initialise();

            Helper.AssertIsNotNull(navigationAction, shouldBeEnabledFilter);

            _navigationAction = navigationAction;

            ButtonWrapper buttonWrapper = GetComponent<ButtonWrapper>();
            Assert.IsNotNull(buttonWrapper);
            buttonWrapper.Initialise(shouldBeEnabledFilter, HandleClick);
        }

        private void HandleClick()
        {
            _navigationAction.Invoke();

            if (Clicked != null)
            {
                Clicked.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
