using BattleCruisers.UI.Sound;
using BattleCruisers.Utils.DataStrctures;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.Scenes.Test.Utilities
{
    public class SoundGroupController : MonoBehaviour
    {
        private ISoundPlayer _soundPlayer;

        // FELIX  Expose in inspector.  If true, allow position to be set via dragging a  game object.
        private bool _playAtObjectLocation;
        
        private ICircularList<AudioClip> _sounds;
        private Text _nameText;

        public List<AudioClip> sounds;
        public int startingSoundIndex;

        private AudioClip _currentSound;
        private AudioClip CurrentSound
        {
            get => _currentSound;
            set
            {
                Assert.IsNotNull(value);
                _currentSound = value;
                _nameText.text = value.name;
            }
        }

        public void Initialise(ISoundPlayer soundPlayer, bool playAtObjectLocation)
        {
            Assert.IsNotNull(soundPlayer);

            _nameText = GetComponentInChildren<Text>();
            Assert.IsNotNull(_nameText);

            _sounds = new CircularList<AudioClip>(sounds)
            {
                Index = startingSoundIndex
            };
            CurrentSound = _sounds.Current();

            _soundPlayer = soundPlayer;
            _playAtObjectLocation = playAtObjectLocation;
        }

        public void PlayOnce()
        {
            if (_playAtObjectLocation)
            {
                // FELIX don't use our position, use player chosen position :)
                _soundPlayer.PlaySound(new AudioClipWrapper(_currentSound), transform.position);
            }
            else
            {
                _soundPlayer.PlaySound(new AudioClipWrapper(_currentSound));
            }
        }

        // FELIX  Merge 2 methods :)
        public void PlayContinuously()
        {
            // FELIX  :D
        }

        public void Stop()
        {
            // FELIX  Only does something for continuously playing sound
        }

        public void NextSound()
        {
            CurrentSound = _sounds.Next();
        }
    }
}