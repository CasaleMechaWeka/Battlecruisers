using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Music
{
    public class MusicPlayer : IMusicPlayer
    {
        private readonly ISingleSoundPlayer _soundPlayer;
        private ISoundKey _currentlyPlaying;

        public MusicPlayer(ISingleSoundPlayer soundPlayer)
        {
            Assert.IsNotNull(soundPlayer);

            _soundPlayer = soundPlayer;
            _currentlyPlaying = null;
        }

        public void PlayScreensSceneMusic()
        {
            PlayMusic(SoundKeys.Music.MainTheme);
        }

        public void PlayVictoryMusic()
        {
            PlayMusic(SoundKeys.Music.Victory);
        }

        public void PlayDefeatMusic()
        {
            PlayMusic(SoundKeys.Music.Defeat, loop: false);
        }

        public void PlayLoadingMusic()
        {
            PlayMusic(SoundKeys.Music.Loading);
        }

        private void PlayMusic(ISoundKey soundKeyToPlay, bool loop = true)
        {
            if (!soundKeyToPlay.Equals(_currentlyPlaying))
            {
                _soundPlayer.PlaySoundAsync(soundKeyToPlay, loop);
                _currentlyPlaying = soundKeyToPlay;
            }
        }

        public void Stop()
        {
            _soundPlayer.Stop();
            _currentlyPlaying = null;
        }
    }
}