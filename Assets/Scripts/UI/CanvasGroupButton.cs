using BattleCruisers.UI.Sound;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI
{
    public abstract class CanvasGroupButton : ElementWithClickSound
    {
        private CanvasGroup _canvasGroup;
        protected override CanvasGroup CanvasGroup => _canvasGroup;

        public override void Initialise(ISoundPlayer soundPlayer)
        {
            base.Initialise(soundPlayer);

            _canvasGroup = GetComponent<CanvasGroup>();
            Assert.IsNotNull(_canvasGroup);
        }
    }
}