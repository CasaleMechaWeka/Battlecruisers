using BattleCruisers.Utils;
using BattleCruisers.Utils.Audio;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Music
{
    public class LayeredMusicPlayer : ILayeredMusicPlayer
    {
        private readonly IAudioVolumeFade _audioVolumeFade;
        private readonly IAudioSource _primarySource, _secondarySource;
        
        public const float FADE_TIME_IN_S = 2;

        public LayeredMusicPlayer(IAudioVolumeFade audioVolumeFade, IAudioSource primarySource, IAudioSource secondarySource)
        {
            Helper.AssertIsNotNull(audioVolumeFade, primarySource, secondarySource);

            _audioVolumeFade = audioVolumeFade;
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
            _audioVolumeFade.FadeToVolume(_secondarySource, targetVolume: 1, FADE_TIME_IN_S);
        }

        public void StopSecondary()
        {
            _audioVolumeFade.FadeToVolume(_secondarySource, targetVolume: 0, FADE_TIME_IN_S);
        }

        public void Stop()
        {
            Assert.IsTrue(_primarySource.IsPlaying, $"{nameof(Stop)} should only be called while playing.");

            _primarySource.Stop();
            _secondarySource.Stop();
        }
    }
}