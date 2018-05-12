using System;
using BattleCruisers.UI.BattleScene.Manager;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Steps.EnemyCruiser
{
    // FELIX  Test
    public class DismissStep : TutorialStep
    {
        private readonly IDismissableEmitter _dismissableEmitter;
        private readonly IUIManagerSettablePermissions _uiManagerPermissions;

        public DismissStep(
            ITutorialStepArgs args, 
            IDismissableEmitter dismissableEmitter,
            IUIManagerSettablePermissions uiManagerPermissions)
            : base(args)
        {
            Assert.IsNotNull(dismissableEmitter);
        
            _dismissableEmitter = dismissableEmitter;
            _uiManagerPermissions = uiManagerPermissions;
        }

        public override void Start(Action completionCallback)
        {
            base.Start(completionCallback);

            _dismissableEmitter.Dismissed += _dismissableEmitter_Dismissed;
            _uiManagerPermissions.CanDismissItemDetails = true;
        }

        private void _dismissableEmitter_Dismissed(object sender, EventArgs e)
        {
            _uiManagerPermissions.CanDismissItemDetails = false;
            OnCompleted();
        }
    }
}
