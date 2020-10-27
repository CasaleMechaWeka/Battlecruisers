using BattleCruisers.UI.Sound;
using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI
{
    public class CanvasGroupButton : ElementWithClickSound
    {
        private CanvasGroup _canvasGroup;
        protected override CanvasGroup CanvasGroup => _canvasGroup;

        public override void Initialise(
            ISingleSoundPlayer soundPlayer, 
            Action clickAction = null,
            IDismissableEmitter parent = null)
        {
            base.Initialise(soundPlayer, clickAction, parent);

            _canvasGroup = GetComponent<CanvasGroup>();
            Assert.IsNotNull(_canvasGroup);
        }
    }
}