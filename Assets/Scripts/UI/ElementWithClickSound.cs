using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;
using BattleCruisers.UI.Sound.Players;
using System;
using UnityEngine.Assertions;
using UnityEngine;

namespace BattleCruisers.UI
{
    public class ElementWithClickSound : ClickableTogglable
    {
        private Action _clickAction;
        private IDismissableEmitter _parent;
        protected ISingleSoundPlayer _soundPlayer;
        protected virtual ISoundKey ClickSound => SoundKeys.UI.Click;

        public virtual void Initialise(
            ISingleSoundPlayer soundPlayer,
            Action clickAction = null,
            IDismissableEmitter parent = null)
        {
            base.Initialise();
            Assert.IsNotNull(soundPlayer);

            _soundPlayer = soundPlayer;
            _clickAction = clickAction;
            _parent = parent;

            if (_parent != null)
            {
                _parent.Dismissed += Parent_Dismissed;
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
                if (_soundPlayer == null)
                    Debug.Log("Sound player is null");
                _soundPlayer.PlaySoundAsync(ClickSound);
            }

            _clickAction?.Invoke();
        }

        protected virtual void DestroySelf()
        {
            if (_parent != null)
            {
                _parent.Dismissed -= Parent_Dismissed;
            }
            Destroy(gameObject);
        }
    }
}