using BattleCruisers.UI.BattleScene.MainMenu;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Steps.WaitSteps
{
    /// <summary>
    /// Completed when the menu is dismissed.  Completes
    /// instantly if the menu is already in dismissed.
    /// </summary>
    public class MenuDismissedWaitStep : TutorialStep
    {
        private readonly IModalMenu _menu;

        public MenuDismissedWaitStep(ITutorialStepArgs args, IModalMenu menu)
            : base(args)
        {
            Assert.IsNotNull(menu);
            _menu = menu;
        }

        public override void Start(Action completionCallback)
        {
            base.Start(completionCallback);

            if (!_menu.IsVisible.Value)
            {
                OnCompleted();
            }
            else
            {
                _menu.IsVisible.ValueChanged += IsVisible_ValueChanged;
            }
        }

        private void IsVisible_ValueChanged(object sender, EventArgs e)
        {
            if (!_menu.IsVisible.Value)
            {
                _menu.IsVisible.ValueChanged -= IsVisible_ValueChanged;
                OnCompleted();
            }
        }
    }
}