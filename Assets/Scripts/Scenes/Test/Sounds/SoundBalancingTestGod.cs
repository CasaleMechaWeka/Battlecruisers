using BattleCruisers.Data.Static;
using BattleCruisers.UI.Music;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils.Fetchers;
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

        public void PlayMusic()
        {
            _musicPlayer.PlayBattleSceneMusic();
        }

        public void StopMusic()
        {
            _musicPlayer.Stop();
        }
    }
}