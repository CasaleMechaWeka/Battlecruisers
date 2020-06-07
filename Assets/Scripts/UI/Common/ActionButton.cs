using BattleCruisers.UI.Sound;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.UI
{
    public class ActionButton : CanvasGroupButton
    {
        private Action _onClick;

        public void Initialise(ISingleSoundPlayer soundPlayer, Action onClick)
        {
            base.Initialise(soundPlayer);

            Assert.IsNotNull(onClick);
            _onClick = onClick;
        }

        protected override void OnClicked()
        {
            base.OnClicked();
            _onClick.Invoke();
        }
    }
}