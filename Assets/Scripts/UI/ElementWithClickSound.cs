using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.UI
{
    public class ElementWithClickSound : ClickableTogglable
    {
        private Action _clickAction;
        protected ISingleSoundPlayer _soundPlayer;
        protected virtual ISoundKey ClickSound => SoundKeys.UI.Click;

        public virtual void Initialise(
            ISingleSoundPlayer soundPlayer, 
            IDismissableEmitter parent = null,
            Action clickAction = null)
        {
            base.Initialise();

            Assert.IsNotNull(soundPlayer);

            _soundPlayer = soundPlayer;
            _clickAction = clickAction;

            if (parent != null)
            {
                parent.Dismissed += Parent_Dismissed;
            }
        }

        private void Parent_Dismissed(object sender, EventArgs e)
        {
            Reset();
        }

        protected override void OnClicked()
        {
            if (ClickSound != null)
            {
                _soundPlayer.PlaySoundAsync(ClickSound);
            }

            _clickAction?.Invoke();
        }
    }
}