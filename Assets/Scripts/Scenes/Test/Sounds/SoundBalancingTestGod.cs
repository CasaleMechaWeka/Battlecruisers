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
        private IMusicPlayer _musicPlayer;

        protected override void Start()
        {
            base.Start();

            _musicPlayer = CreateMusicPlayer();
            _musicPlayer.LevelMusicKey = SoundKeys.Music.Background.Kentient;

            SetupSoundPlayerObjects();
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
            _musicPlayer.PlayBattleSceneMusic();
        }

        public void PlayDangerMusic()
        {
            _musicPlayer.PlayDangerMusic();
        }

        public void StopMusic()
        {
            _musicPlayer.Stop();
        }
    }
}