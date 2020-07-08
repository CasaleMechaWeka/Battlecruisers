using BattleCruisers.Utils;
using BattleCruisers.Utils.Audio;
using BattleCruisers.Utils.PlatformAbstractions.Audio;

namespace BattleCruisers.UI.Music
{
    public class LayeredMusicPlayer : ILayeredMusicPlayer
    {
        private readonly IAudioVolumeFade _audioVolumeFade;
        private readonly IAudioSource _primarySource, _secondarySource;
        
        public const float FADE_TIME_IN_S = 2;
        public const float MAX_VOLUME = 0.75f;

        public LayeredMusicPlayer(IAudioVolumeFade audioVolumeFade, IAudioSource primarySource, IAudioSource secondarySource)
        {
            Helper.AssertIsNotNull(audioVolumeFade, primarySource, secondarySource);

            _audioVolumeFade = audioVolumeFade;
            _primarySource = primarySource;
            _secondarySource = secondarySource;
        }

        public void Play()
        {
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
            _audioVolumeFade.FadeToVolume(_secondarySource, targetVolume: MAX_VOLUME, FADE_TIME_IN_S);
        }

        public void StopSecondary()
        {
            _audioVolumeFade.FadeToVolume(_secondarySource, targetVolume: 0, FADE_TIME_IN_S);
        }

        public void Stop()
        {
            if (!_primarySource.IsPlaying)
            {
                Logging.Log(Tags.SOUND, $"No sound is playing, returning.");
                return;
            }

            _primarySource.Stop();
            _secondarySource.Stop();
        }
    }
}