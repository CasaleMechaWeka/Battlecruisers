using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils.UIWrappers
{
    public class AudioSourceWrapper : IAudioSourceWrapper
    {
        private readonly AudioSource _audioSource;

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

        public void Play(bool loop = false)
        {
            _audioSource.loop = loop;
            _audioSource.Play();
        }

        public void Stop()
        {
            _audioSource.Stop();
        }
    }
}
