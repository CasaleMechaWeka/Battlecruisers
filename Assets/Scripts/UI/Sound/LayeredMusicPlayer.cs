using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Sound
{
    // FELIX  use, test
    public class LayeredMusicPlayer : ILayeredMusicPlayer
    {
        private readonly IAudioSource _primarySource, _secondarySource;
        private readonly IAudioClipWrapper _primarySound, _secondarySound;

        public LayeredMusicPlayer(
            IAudioSource primarySource, 
            IAudioSource secondarySource, 
            ISoundFetcher soundFetcher,
            ISoundKey primarySoundKey, 
            ISoundKey secondarySoundKey)
        {
            Helper.AssertIsNotNull(primarySource, secondarySource, soundFetcher, primarySoundKey, secondarySoundKey);

            _primarySource = primarySource;
            _primarySource.AudioClip = soundFetcher.GetSound(primarySoundKey);

            _secondarySource = secondarySource;
            _secondarySource.AudioClip = soundFetcher.GetSound(secondarySoundKey);
        }

        public void Play()
        {
            Assert.IsFalse(_primarySource.IsPlaying, $"{nameof(Play)} should only be called while not playing.");

            _primarySource.Volume = 1;
            _primarySource.Play(isSpatial: false, loop: true);

            _secondarySource.Volume = 0;
            _secondarySource.Play(isSpatial: false, loop: true);
        }

        public void PlaySecondary()
        {
            _secondarySource.Volume = 1;
        }

        public void StopSecondary()
        {
            _secondarySource.Volume = 0;
        }

        public void Stop()
        {
            Assert.IsTrue(_primarySource.IsPlaying, $"{nameof(Stop)} should only be called while playing.");

            _primarySource.Stop();
            _secondarySource.Stop();
        }
    }
}