using BattleCruisers.UI.Sound;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace BattleCruisers.Scenes.Test.Utilities
{
    public class SoundPlayerController : MonoBehaviour, IPointerClickHandler
    {
        private ISoundPlayer _soundPlayer;
        private ISoundKey _soundKey;

        public SoundType soundType;
        public string soundName;
        public bool playAtObjectLocation;

        public void Initialise(ISoundPlayer soundPlayer)
        {
            Assert.IsNotNull(soundPlayer);

            _soundPlayer = soundPlayer;
            _soundKey = new SoundKey(soundType, soundName);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (playAtObjectLocation)
            {
                _soundPlayer.PlaySound(_soundKey, transform.position);
            }
            else
            {
                // Play sound at camera location (always full volume)
                _soundPlayer.PlaySound(_soundKey);
            }
        }
    }
}