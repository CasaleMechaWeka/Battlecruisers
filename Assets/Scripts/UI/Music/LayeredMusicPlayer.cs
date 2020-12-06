using BattleCruisers.Utils;
using BattleCruisers.Utils.Audio;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Music
{
    public class LayeredMusicPlayer : ILayeredMusicPlayer
    {
        private readonly IAudioVolumeFade _audioVolumeFade;
        private readonly IAudioSource _primarySource, _secondarySource;
        private bool _isDisposed;
        
        public const float FADE_TIME_IN_S = 2;
        public const float MAX_VOLUME = 1;

        public LayeredMusicPlayer(IAudioVolumeFade audioVolumeFade, IAudioSource primarySource, IAudioSource secondarySource)
        {
            Helper.AssertIsNotNull(audioVolumeFade, primarySource, secondarySource);

            _audioVolumeFade = audioVolumeFade;
            _primarySource = primarySource;
            _secondarySource = secondarySource;
            _isDisposed = false;
        }

        public void Play()
        {
            Assert.IsFalse(_isDisposed);

            if (_primarySource.IsPlaying)
            {
                Logging.Log(Tags.SOUND, $"Prmary source already playing, returning.");
                return;
            }

            _primarySource.Volume = MAX_VOLUME;
            _primarySource.Play(isSpatial: false, loop: true);

            _secondarySource.Volume = 0;
            _secondarySource.Play(isSpatial: false, loop: true);
        }

        public void PlaySecondary()
        {
            Assert.IsFalse(_isDisposed);
            _audioVolumeFade.FadeToVolume(_secondarySource, targetVolume: MAX_VOLUME, FADE_TIME_IN_S);
        }

        public void StopSecondary()
        {
            Assert.IsFalse(_isDisposed);
            _audioVolumeFade.FadeToVolume(_secondarySource, targetVolume: 0, FADE_TIME_IN_S);
        }

        public void Stop()
        {
            Assert.IsFalse(_isDisposed);

            if (!_primarySource.IsPlaying)
            {
                Logging.Log(Tags.SOUND, $"No sound is playing, returning.");
                return;
            }

            _primarySource.Stop();
            _secondarySource.Stop();
        }

        // FELIX  Test :P
        public void DisposeManagedState()
        {
            if (!_isDisposed)
            {
                _primarySource.FreeAudioClip();
                _secondarySource.FreeAudioClip();
                _isDisposed = true;
            }
        }
    }
}