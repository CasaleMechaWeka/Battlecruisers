using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.Utils.DataStrctures;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        private Text _nameText, _foreverButtonText;
        private bool _playingForever;

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

            _nameText = transform.FindNamedComponent<Text>("SoundName");
            _foreverButtonText = transform.FindNamedComponent<Text>("ButtonsPanel/InfinitePlayButton/Text");

            _sounds = new CircularList<AudioClip>(sounds)
            {
                Index = startingSoundIndex
            };
            CurrentSound = _sounds.Current();

            _soundPlayer = soundPlayer;
            _playAtObjectLocation = playAtObjectLocation;
            _playingForever = false;
        }

        public void PlayOnce()
        {
            Logging.LogMethod(Tags.ALWAYS);

            if (_playAtObjectLocation)
            {
                // FELIX don't use our position, use player chosen position :)
                _soundPlayer.PlaySound(new AudioClipWrapper(CurrentSound), transform.position);
            }
            else
            {
                _soundPlayer.PlaySound(new AudioClipWrapper(CurrentSound));
            }
        }

        public void InfinitoPlayToggle()
        {
            if (_playingForever)
            {
                Stop();
            }
            else
            {
                PlayForever();
            }
        }

        private async void PlayForever()
        {
            _playingForever = true;
            _foreverButtonText.text = "Stop";

            while (_playingForever)
            {
                PlayOnce();
                await Task.Delay((int)(CurrentSound.length + 0.25f) * 1000);
            }
        }

        private void Stop()
        {
            _playingForever = false;
            _foreverButtonText.text = "Play loop";
        }

        public void NextSound()
        {
            Logging.LogMethod(Tags.ALWAYS);
            CurrentSound = _sounds.Next();
        }
    }
}