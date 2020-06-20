using System;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.BattleScene.Presentables;
using BattleCruisers.UI.Sound;
using UnityEngine.Assertions;

namespace BattleCruisers.UI
{
    public class ElementWithClickSound : ClickableTogglable
    {
        protected ISingleSoundPlayer _soundPlayer;
        protected virtual ISoundKey ClickSound => SoundKeys.UI.Click;

        public virtual void Initialise(ISingleSoundPlayer soundPlayer, IPresentable parent = null)
        {
            base.Initialise();

            Assert.IsNotNull(soundPlayer);
            _soundPlayer = soundPlayer;

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
        }
    }
}