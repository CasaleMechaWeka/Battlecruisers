using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils.PlatformAbstractions.UI
{
    // FELIX  Test this is heard regardless of where in the scene the camera is!!
    public class AwaitableAudioSource : IAwaitableAudioSource
    {
        private readonly AudioSource _audioSource;

        public AwaitableAudioSource(AudioSource audioSource)
        {
            Assert.IsNotNull(audioSource);

            _audioSource = audioSource;
            _audioSource.spatialBlend = AudioSourceBC.MIN_BLEND;
            _audioSource.loop = false;
        }

        public IEnumerator Play(IAudioClipWrapper audioClip)
        {
            Assert.IsNotNull(audioClip);

            _audioSource.clip = audioClip.AudioClip;
            _audioSource.Play();

            yield return new WaitForSeconds(audioClip.AudioClip.length);
        }
    }
}