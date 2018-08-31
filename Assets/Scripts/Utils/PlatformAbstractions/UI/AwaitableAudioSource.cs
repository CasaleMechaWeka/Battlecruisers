using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils.PlatformAbstractions.UI
{
    // FELIX  Test this is heard regardless of where in the scene the camera is!!
    // FELIX  Rename :/
    public class AwaitableAudioSource : MonoBehaviour, IAwaitableAudioSource
    {
        private readonly AudioSource _audioSource;
        private Action _audioCompletedCallback;

        // FELIX  Don't inject, use GetComponent?
        public AwaitableAudioSource(AudioSource audioSource)
        {
            Assert.IsNotNull(audioSource);

            _audioSource = audioSource;
            _audioSource.spatialBlend = AudioSourceBC.MIN_BLEND;
            _audioSource.loop = false;
        }

        public void Play(IAudioClipWrapper audioClip, Action audioCompletedCallback)
        {
            Helper.AssertIsNotNull(audioClip, audioCompletedCallback);
            Assert.IsNull(_audioCompletedCallback, "Called Play() before last audio completed :/");

            _audioSource.clip = audioClip.AudioClip;
            _audioCompletedCallback = audioCompletedCallback;
            _audioSource.Play();

            Invoke("ExecuteCallback", audioClip.AudioClip.length);
        }

        private void ExecuteCallback()
        {
            _audioCompletedCallback.Invoke();
            _audioCompletedCallback = null;
        }
    }
}