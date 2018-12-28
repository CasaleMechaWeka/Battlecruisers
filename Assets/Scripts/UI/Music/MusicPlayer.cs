using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Music
{
    public class MusicPlayer : IMusicPlayer
    {
        private readonly ISingleSoundPlayer _soundPlayer;
        private ISoundKey _currentlyPlaying;

        public ISoundKey LevelMusicKey { private get; set; }

        public MusicPlayer(ISingleSoundPlayer soundPlayer)
        {
            Assert.IsNotNull(soundPlayer);

            _soundPlayer = soundPlayer;
            _currentlyPlaying = null;
            LevelMusicKey = null;
        }

        public void PlayScreensSceneMusic()
        {
            PlayMusic(SoundKeys.Music.MainTheme);
        }

        public void PlayBattleSceneMusic()
        {
            Assert.IsNotNull(LevelMusicKey);
            PlayMusic(LevelMusicKey);
        }

        public void PlayDangerMusic()
        {
            PlayMusic(SoundKeys.Music.Danger);
        }

        public void PlayVictoryMusic()
        {
            PlayMusic(SoundKeys.Music.Victory);
        }

        private void PlayMusic(ISoundKey soundKeyToPlay)
        {
            if (!soundKeyToPlay.Equals(_currentlyPlaying))
            {
                _soundPlayer.PlaySound(soundKeyToPlay, loop: true);
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