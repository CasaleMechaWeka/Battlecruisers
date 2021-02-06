using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Assertions;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace BattleCruisers.UI.Music
{
    public class MusicPlayer : IMusicPlayer
    {
        private readonly ISingleSoundPlayer _soundPlayer;
        private ISoundKey _currentlyPlaying;
        private AsyncOperationHandle<AudioClip> _currentlyPlayingHandle;

        public float Volume
        {
            set => _soundPlayer.Volume = value;
        }

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
            PlayMusic(SoundKeys.Music.Victory, loop: false);
        }

        public void PlayDefeatMusic()
        {
            PlayMusic(SoundKeys.Music.Defeat, loop: false);
        }

        public void PlayTrashMusic()
        {
            PlayMusic(SoundKeys.Music.TrashTalk, loop: false);
        }

        private async Task PlayMusic(ISoundKey soundKeyToPlay, bool loop = true)
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