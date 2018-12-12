using System;
using BattleCruisers.UI.BattleScene.Manager;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Steps
{
    public class HideItemDetailsStep : TutorialStepNEW
    {
        private readonly IUIManager _uiManager;

        public HideItemDetailsStep(ITutorialStepArgsNEW args, IUIManager uiManager) 
            : base(args)
        {
            Assert.IsNotNull(uiManager);
            _uiManager = uiManager;
        }

        public override void Start(Action completionCallback)
        {
            base.Start(completionCallback);

            _uiManager.HideItemDetails();
            OnCompleted();
        }
    }
}