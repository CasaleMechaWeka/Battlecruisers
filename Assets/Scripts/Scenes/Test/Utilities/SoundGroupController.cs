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
        private ICircularList<AudioClip> _sounds;
        private Text _nameText, _locationText, _foreverButtonText;
        private bool _playingForever;

        public List<AudioClip> sounds;
        public int startingSoundIndex;
        public bool playAtLocation = false;

        [DrawIf("playAtLocation", true)] 
        public GameObject playLocation;

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

        public void Initialise(ISoundPlayer soundPlayer)
        {
            Assert.IsTrue(!playAtLocation || playLocation != null);
            Assert.IsNotNull(soundPlayer);

            _nameText = transform.FindNamedComponent<Text>("TextPanel/SoundName");
            _locationText = transform.FindNamedComponent<Text>("TextPanel/PlayLocation");
            _foreverButtonText = transform.FindNamedComponent<Text>("ButtonsPanel/InfinitePlayButton/Text");

            _sounds = new CircularList<AudioClip>(sounds)
            {
                Index = startingSoundIndex
            };
            CurrentSound = _sounds.Current();

            _soundPlayer = soundPlayer;
            _playingForever = false;
            _locationText.text = playLocation?.name ?? "(not spatial)";
        }

        public void PlayOnce()
        {
            Logging.LogMethod(Tags.ALWAYS);

            if (playAtLocation)
            {
                _soundPlayer.PlaySound(new AudioClipWrapper(CurrentSound), playLocation.transform.position);
            }
            else
            {
                _soundPlayer.PlaySound(new AudioClipWrapper(CurrentSound));
            }
        }

        public void InfinitoPlayToggle()
        {
            Logging.LogMethod(Tags.ALWAYS);
            
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
            Logging.LogMethod(Tags.ALWAYS);
            
            _playingForever = true;
            _foreverButtonText.text = "Stop";

            while (
                this != null // ensure not destroyed
                && _playingForever)
            {
                PlayOnce();
                await Task.Delay((int)(CurrentSound.length + 0.25f) * 1000);
            }
        }

        private void Stop()
        {
            Logging.LogMethod(Tags.ALWAYS);

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