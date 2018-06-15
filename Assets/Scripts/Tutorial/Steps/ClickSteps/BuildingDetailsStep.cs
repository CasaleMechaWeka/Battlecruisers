using System;
using BattleCruisers.Buildables;
using BattleCruisers.Tutorial.Providers;
using BattleCruisers.Tutorial.Steps.Providers;
using BattleCruisers.UI.BattleScene.Manager;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Steps.ClickSteps
{
    public class BuildingDetailsStep : ClickStep
    {
        private readonly IItemProvider<IBuildable> _buildableProvider;
        private readonly IUIManagerSettablePermissions _uiManagerPermissions;

        public BuildingDetailsStep(
            ITutorialStepArgs args, 
            ISingleBuildableProvider buildableProvider,
            IUIManagerSettablePermissions uiManagerPermissions) 
            : base(args, buildableProvider)
        {
            Assert.IsNotNull(uiManagerPermissions);

            _buildableProvider = buildableProvider;
            _uiManagerPermissions = uiManagerPermissions;
        }

		public override void Start(Action completionCallback)
		{
            base.Start(completionCallback);

            _uiManagerPermissions.CanShowItemDetails = true;
		}

		protected override void OnCompleted()
		{
            _uiManagerPermissions.CanShowItemDetails = false;

			base.OnCompleted();
		}
	}
}
