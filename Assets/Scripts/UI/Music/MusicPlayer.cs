using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.Music
{
    // FELIX  Update tests :)
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
            PlayMusic(Music.ScreensScene, _musicProvider.ScreensSceneKey);
        }

        public void PlayBattleSceneMusic()
        {
            PlayMusic(Music.BattleScene, _musicProvider.BattleSceneKey);
        }

        public void PlayDangerMusic()
        {
            PlayMusic(Music.Danger, _musicProvider.DangerKey);
        }

        public void PlayVictoryMusic()
        {
            PlayMusic(Music.Victory, _musicProvider.VictoryKey);
        }

        private void PlayMusic(Music musicToPlay, ISoundKey soundKeyToPlay)
        {
            if (musicToPlay != _currentlyPlaying)
            {
                _soundPlayer.PlaySound(soundKeyToPlay, loop: true);
                _currentlyPlaying = musicToPlay;
            }
        }

        public void Stop()
        {
            _soundPlayer.Stop();
            _currentlyPlaying = null;
        }
    }
}