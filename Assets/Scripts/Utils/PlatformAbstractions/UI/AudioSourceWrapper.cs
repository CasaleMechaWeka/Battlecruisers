using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils.PlatformAbstractions.UI
{
    public class AudioSourceWrapper : IAudioSourceWrapper
    {
        private readonly AudioSource _audioSource;

        private const float MAX_BLEND = 1;
        private const float MIN_BLEND = 0;

        public IAudioClipWrapper AudioClip
        {
            set
            {
                _audioSource.clip = value != null ? value.AudioClip : null;
            }
        }

        public AudioSourceWrapper(AudioSource audioSource)
        {
            Assert.IsNotNull(audioSource);
            _audioSource = audioSource;
        }

        public void Play(bool isSpatial = true, bool loop = false)
        {
            _audioSource.spatialBlend = isSpatial ? MAX_BLEND : MIN_BLEND;
            _audioSource.loop = loop;
            _audioSource.Play();
        }

        public void Stop()
        {
            // Only stop audio if game object is still alive
            if (_audioSource.gameObject != null)
            {
                _audioSource.Stop();
            }
        }
    }
}
