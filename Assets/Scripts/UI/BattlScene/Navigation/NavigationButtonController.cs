using System;
using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.Navigation
{
    public class NavigationButtonController : UIElement, IButton
    {
        private Action _navigationAction;
        private IActivenessDecider _activenessDecider;
        private ButtonWrapper _buttonWrapper;

        public event EventHandler Clicked;

        public void Initialise(Action navigationAction, IActivenessDecider activenessDecider)
        {
            base.Initialise();

            Helper.AssertIsNotNull(navigationAction, activenessDecider);

            _navigationAction = navigationAction;
            _activenessDecider = activenessDecider;
            _activenessDecider.PotentialActivenessChange += _activenessDecider_PotentialActivenessChange;

            _buttonWrapper = GetComponent<ButtonWrapper>();
            Assert.IsNotNull(_buttonWrapper);
            _buttonWrapper.Initialise(Handleclick);

            UpdateActiveness();
        }

        private void _activenessDecider_PotentialActivenessChange(object sender, EventArgs e)
        {
            UpdateActiveness();
        }

        private void UpdateActiveness()
        {
            _buttonWrapper.IsEnabled = _activenessDecider.ShouldBeEnabled;
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
