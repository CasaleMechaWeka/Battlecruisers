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
        private Text _nameText, _locationText, _foreverButtonText, _playAllButtonText;
        private bool _playingForever, _playingAll;

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
                _nameText.text = $"{value.name} ({_sounds.Index + 1}/{_sounds.Items.Count})";
            }
        }

        public void Initialise(ISoundPlayer soundPlayer)
        {
            Assert.IsTrue(!playAtLocation || playLocation != null);
            Assert.IsNotNull(soundPlayer);

            _nameText = transform.FindNamedComponent<Text>("TextPanel/SoundName");
            _locationText = transform.FindNamedComponent<Text>("TextPanel/PlayLocation");
            _foreverButtonText = transform.FindNamedComponent<Text>("ButtonsPanel/InfinitePlayButton/Text");
            _playAllButtonText = transform.FindNamedComponent<Text>("ButtonsPanel/PlayAllButton/Text");

            _sounds = new CircularList<AudioClip>(sounds)
            {
                Index = startingSoundIndex
            };
            CurrentSound = _sounds.Current();

            _soundPlayer = soundPlayer;
            _playingForever = false;
            _playingAll = false;
            _locationText.text = playAtLocation ? playLocation.name : "(not spatial)";
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

            StopPlayAll();

            if (_playingForever)
            {
                StopRepeating();
            }
            else
            {
                PlayRepeating();
            }
        }

        private async void PlayRepeating()
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

        private void StopRepeating()
        {
            Logging.LogMethod(Tags.ALWAYS);

            _playingForever = false;
            _foreverButtonText.text = "Play loop";
        }

        public void PlayAllToggle()
        {
            Logging.LogMethod(Tags.ALWAYS);

            StopRepeating();

            if (_playingAll)
            {
                StopPlayAll();
            }
            else
            {
                PlayAll();
            }
        }

        private async void PlayAll()
        {
            Logging.LogMethod(Tags.ALWAYS);

            _playingAll = true;
            _playAllButtonText.text = "Stop";

            while (true)
            {
                PlayOnce();
                await Task.Delay((int)(CurrentSound.length + 0.25f) * 1000);

                if (this == null // destroyed
                    || !_playingAll)
                {
                    break;
                }

                NextSound();
            }
        }

        private void StopPlayAll()
        {
            Logging.LogMethod(Tags.ALWAYS);

            _playingAll = false;
            _playAllButtonText.text = "Play all";
        }

        public void NextSound()
        {
            Logging.LogMethod(Tags.ALWAYS);
            CurrentSound = _sounds.Next();
        }
    }
}