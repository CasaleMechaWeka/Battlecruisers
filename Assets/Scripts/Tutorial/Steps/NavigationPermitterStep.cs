using System;
using BattleCruisers.UI.BattleScene.Navigation;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Steps.EnemyCruiser
{
	public class NavigationPermitterStep : TutorialStep
    {
		private readonly INavigationSettings _navigationSettings;
        private readonly NavigationPermission _permission;

        public NavigationPermitterStep(
            ITutorialStepArgs args, 
			INavigationSettings navigationSettings,
            NavigationPermission permission)
            : base(args)
        {
			Assert.IsNotNull(navigationSettings);

			_navigationSettings = navigationSettings;
			_permission = permission;
        }

        public override void Start(Action completionCallback)
        {
            base.Start(completionCallback);

			_navigationSettings.Permission = _permission;

            OnCompleted();
        }
    }
}
