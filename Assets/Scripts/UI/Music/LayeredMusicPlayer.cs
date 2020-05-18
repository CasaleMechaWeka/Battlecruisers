using BattleCruisers.Utils;
using BattleCruisers.Utils.Audio;
using BattleCruisers.Utils.PlatformAbstractions.UI;

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

        // FELIX  Update tests
        public void Play()
        {
            if (_primarySource.IsPlaying)
            {
                Logging.Log(Tags.SOUND, $"Prmary source already playing, returning.");
                return;
            }

            _primarySource.Volume = 1;
            _primarySource.Play(isSpatial: false, loop: true);

            _secondarySource.Volume = 0;
            _secondarySource.Play(isSpatial: false, loop: true);
        }

        public void PlaySecondary()
        {
            if (!_primarySource.IsPlaying)
            {
                Logging.Log(Tags.SOUND, $"No point playing secondary if primary is not playing, returning.");
                return;
            }

            _audioVolumeFade.FadeToVolume(_secondarySource, targetVolume: 1, FADE_TIME_IN_S);
        }

        public void StopSecondary()
        {
            if (!_secondarySource.IsPlaying)
            {
                Logging.Log(Tags.SOUND, $"Secondary is not playing, returning.");
                return;
            }

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