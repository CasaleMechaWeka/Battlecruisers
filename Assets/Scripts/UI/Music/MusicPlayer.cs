using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;
using BattleCruisers.UI.Sound.Players;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Assertions;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace BattleCruisers.UI.Music
{
    public class MusicPlayer : IMusicPlayer
    {
        private readonly SingleSoundPlayer _soundPlayer;
        private SoundKey _currentlyPlaying;
        private AsyncOperationHandle<AudioClip> _currentlyPlayingHandle;

        public MusicPlayer(SingleSoundPlayer soundPlayer)
        {
            Assert.IsNotNull(soundPlayer);

            _soundPlayer = soundPlayer;
            _currentlyPlaying = null;
        }

        public void PlayScreensSceneMusic()
        {
            _ = PlayMusic(SoundKeys.Music.MainTheme);
        }

        public void PlayVictoryMusic()
        {
            _ = PlayMusic(SoundKeys.Music.Victory, loop: false);
        }

        public void PlayDefeatMusic()
        {
            _ = PlayMusic(SoundKeys.Music.Defeat, loop: false);
        }

        public void PlayTrashMusic()
        {
            _ = PlayMusic(SoundKeys.Music.TrashTalk, loop: false);
        }

        public void PlayCutsceneMusic()
        {
            _ = PlayMusic(SoundKeys.Music.Cutscene, loop: false);
        }

        public void PlayCreditsMusic()
        {
            _ = PlayMusic(SoundKeys.Music.Credits, loop: false);
        }

        public void PlayAdsMusic()
        {
            _ = PlayMusic(SoundKeys.Music.Advertisements, loop: false);
        }

        private async Task PlayMusic(SoundKey soundKeyToPlay, bool loop = true)
        {
            if (!soundKeyToPlay.Equals(_currentlyPlaying))
            {
                Stop();

                _currentlyPlaying = soundKeyToPlay;
                _currentlyPlayingHandle = await _soundPlayer.PlaySoundAsync(soundKeyToPlay, loop);
            }
        }

        public void Stop()
        {
            _soundPlayer.Stop();

            if (_currentlyPlaying != null)
            {
                // Free previously playing music.  Music files are large.
                Addressables.Release(_currentlyPlayingHandle);
            }

            _currentlyPlaying = null;
        }
    }
}