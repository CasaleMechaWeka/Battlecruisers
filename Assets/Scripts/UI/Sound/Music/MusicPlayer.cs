using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using System;

namespace BattleCruisers.UI.Music
{
    // FELIX  Test :)
    public class MusicPlayer : IMusicPlayer
    {
        private readonly IMusicProvider _musicProvider;
        private readonly ISingleSoundPlayer _soundPlayer;
        private Music? _currentlyPlaying;

        public MusicPlayer(IMusicProvider musicProvider, ISingleSoundPlayer soundPlayer)
        {
            Helper.AssertIsNotNull(musicProvider, soundPlayer);

            _musicProvider = musicProvider;
            _soundPlayer = soundPlayer;
            _currentlyPlaying = null;
        }

        public void PlayScreensSceneMusic()
        {
            PlayMusic(Music.ScreensScene);
        }

        public void PlayBattleSceneMusic()
        {
            PlayMusic(Music.BattleScene);
        }

        public void PlayDangerMusic()
        {
            PlayMusic(Music.Danger);
        }

        public void PlayVictoryMusic()
        {
            PlayMusic(Music.Victory);
        }

        private void PlayMusic(Music musicToPlay)
        {
            if (musicToPlay != _currentlyPlaying)
            {
                ISoundKey soundKeyToPlay = GetKey(musicToPlay);
                _soundPlayer.PlaySound(soundKeyToPlay, loop: true);
                _currentlyPlaying = musicToPlay;
            }
        }

        private ISoundKey GetKey(Music musicToPlay)
        {
            switch (musicToPlay)
            {
                case Music.BattleScene:
                    return _musicProvider.BattleSceneKey;

                case Music.ScreensScene:
                    return _musicProvider.ScreensSceneKey;

                case Music.Danger:
                    return _musicProvider.DangerKey;

                case Music.Victory:
                    return _musicProvider.VictoryKey;

                default:
                    throw new ArgumentException();
            }
        }
    }
}