using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Music
{
    // FELIX  use, test
    public class LayeredMusicPlayer : ILayeredMusicPlayer
    {
        private readonly IAudioSource _primarySource, _secondarySource;

        public LayeredMusicPlayer(IAudioSource primarySource, IAudioSource secondarySource)
        {
            Helper.AssertIsNotNull(primarySource, secondarySource);

            _primarySource = primarySource;
            _secondarySource = secondarySource;
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