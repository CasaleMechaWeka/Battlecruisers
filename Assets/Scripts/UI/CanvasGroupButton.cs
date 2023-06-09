using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI
{
    public class CanvasGroupButton : ElementWithClickSound
    {
        private CanvasGroup _canvasGroup;
        protected override CanvasGroup CanvasGroup => _canvasGroup;
        public bool hasFeedback;

        public override void Initialise(
            ISingleSoundPlayer soundPlayer,
            Action clickAction = null,
            IDismissableEmitter parent = null)
        {
            base.Initialise(soundPlayer, clickAction, parent);

            _canvasGroup = GetComponent<CanvasGroup>();
            if (hasFeedback)
                _selectedFeedbackIcon = transform.FindNamedComponent<Transform>("SelectedFeedback").gameObject;
            Assert.IsNotNull(_canvasGroup);
        }
    }
}