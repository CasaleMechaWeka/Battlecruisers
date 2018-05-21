using System;
using BattleCruisers.Tutorial.Providers;
using BattleCruisers.UI.BattleScene.Navigation;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Steps.ClickSteps
{
	// FELIX  Rename to NavigationButtonStep
	public class NavigationStep : ClickStep
    {
		private readonly INavigationSettings _navigationSettings;

		public NavigationStep(ITutorialStepArgs args, INavigationSettings navigationSettings, params IClickableEmitter[] completionClickables)
            : base(args, new StaticListProvider<IClickableEmitter>(completionClickables))
        {
			Assert.IsNotNull(navigationSettings);
            _navigationSettings = navigationSettings;
        }

        public override void Start(Action completionCallback)
        {
            base.Start(completionCallback);
			_navigationSettings.Permission = NavigationPermission.TransitionsOnly;
        }

        protected override void OnCompleted()
        {
			_navigationSettings.Permission = NavigationPermission.None;
            base.OnCompleted();
        }
    }
}
