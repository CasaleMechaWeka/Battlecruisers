using BattleCruisers.UI.Sound;
using UnityEngine.Assertions;

namespace BattleCruisers.UI
{
    public class ButtonWithClickSound : ClickableTogglable
    {
        private ISoundPlayer _soundPlayer;
        protected virtual ISoundKey ClickSound => null;

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