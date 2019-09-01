using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;
using UnityEngine.Assertions;

namespace BattleCruisers.UI
{
    public class ElementWithClickSound : ClickableTogglable
    {
        protected ISoundPlayer _soundPlayer;
        protected virtual ISoundKey ClickSound => SoundKeys.UI.Click;

        public virtual void Initialise(ISoundPlayer soundPlayer)
        {
            base.Initialise();

            Assert.IsNotNull(soundPlayer);
            _soundPlayer = soundPlayer;
        }

        protected override void OnClicked()
        {
            if (ClickSound != null)
            {
                _soundPlayer.PlaySound(ClickSound);
            }
        }
    }
}