using BattleCruisers.UI.Sound;
using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI
{
    public abstract class CanvasGroupButton : ElementWithClickSound
    {
        private CanvasGroup _canvasGroup;
        protected override CanvasGroup CanvasGroup => _canvasGroup;

        public override void Initialise(
            ISingleSoundPlayer soundPlayer, 
            IDismissableEmitter parent = null,
            Action clickAction = null)
        {
            base.Initialise(soundPlayer, parent, clickAction);

            _canvasGroup = GetComponent<CanvasGroup>();
            Assert.IsNotNull(_canvasGroup);
        }
    }
}