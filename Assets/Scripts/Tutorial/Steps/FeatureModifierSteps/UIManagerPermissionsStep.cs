using BattleCruisers.UI.BattleScene.Manager;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Steps.FeatureModifierSteps
{
    // FELIX  Test :D
    public class UIManagerPermissionsStep : TutorialStepNEW
    {
        private readonly IUIManagerSettablePermissions _permissions;
        private readonly bool _canShowItemDetails;
        private readonly bool _canDismissItemDetails;

        public UIManagerPermissionsStep(
            ITutorialStepArgsNEW args,
            IUIManagerSettablePermissions permissions, 
            bool canShowItemDetails, 
            bool canDismissItemDetails)
            : base(args)
        {
            Assert.IsNotNull(permissions);

            _permissions = permissions;
            _canShowItemDetails = canShowItemDetails;
            _canDismissItemDetails = canDismissItemDetails;
        }

        public override void Start(Action completionCallback)
        {
            base.Start(completionCallback);

            _permissions.CanShowItemDetails = _canShowItemDetails;
            _permissions.CanShowItemDetails = _canShowItemDetails;

            OnCompleted();
        }
    }
}