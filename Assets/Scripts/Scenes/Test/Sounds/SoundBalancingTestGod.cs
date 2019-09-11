using BattleCruisers.Data.Static;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.UI.Music;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.PlatformAbstractions;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test.Sounds
{
    public class SoundBalancingTestGod : NavigationTestGod
    {
        private ILayeredMusicPlayer _musicPlayer;
        private AudioListener _audioListener;

        protected override void Start()
        {
            base.Start();

            LayeredMusicPlayerInitialiser musicInitialiser = GetComponentInChildren<LayeredMusicPlayerInitialiser>();
            Assert.IsNotNull(musicInitialiser);
            _musicPlayer 
                = musicInitialiser.CreatePlayer(
                    new SoundFetcher(),
                    SoundKeys.Music.Background.KentientBase,
                    SoundKeys.Music.Background.KentientDanger);

            SetupSoundPlayerObjects();

            _audioListener = Camera.main.GetComponent<AudioListener>();
            Assert.IsNotNull(_audioListener);
        }

        private void SetupSoundPlayerObjects()
        {
            ISoundPlayer soundPlayer
                = new SoundPlayer(
                    new SoundFetcher(),
                    new AudioClipPlayer(),
                    new CameraBC(Camera.main));

            SoundPlayerController[] soundPlayerObjects = FindObjectsOfType<SoundPlayerController>();

            foreach (SoundPlayerController soundPlayerObject in soundPlayerObjects)
            {
                soundPlayerObject.Initialise(soundPlayer);
            }
        }

        private IMusicPlayer CreateMusicPlayer()
        {
            AudioSource platformAudioSource = GetComponent<AudioSource>();
            Assert.IsNotNull(platformAudioSource);
            IAudioSource audioSource = new AudioSourceBC(platformAudioSource);

            return
                new MusicPlayer(
                    new SingleSoundPlayer(
                        new SoundFetcher(),
                        audioSource));
        }

        public void PlayBackgroundMusic()
        {
            _musicPlayer.Play();
        }

        public void PlayDangerMusic()
        {
            _musicPlayer.PlaySecondary();
        }

        public void StopMusic()
        {
            _musicPlayer.Stop();
        }

        protected override void Update()
        {
            base.Update();

            Debug.Log($"Audio listener position: {_audioListener.transform.position}");
        }
    }
}