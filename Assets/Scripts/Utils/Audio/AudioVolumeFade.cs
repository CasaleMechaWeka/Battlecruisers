using BattleCruisers.Utils.PlatformAbstractions.UI;
using BattleCruisers.Utils.Threading;
using System.Collections;
using UnityCommon.PlatformAbstractions.Time;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils.Audio
{
    public class AudioVolumeFade : IAudioVolumeFade
    {
        private readonly ICoroutineStarter _coroutineStarter;
        private readonly ITime _time;

        public AudioVolumeFade(ICoroutineStarter coroutineStarter, ITime time)
        {
            Helper.AssertIsNotNull(coroutineStarter, time);

            _coroutineStarter = coroutineStarter;
            _time = time;
        }

        public void FadeToVolume(IAudioSource audioSource, float targetVolume, float durationInS)
        {
            Assert.IsNotNull(audioSource);

            _coroutineStarter.StartRoutine(FadeToVolumeCoroutine(audioSource, targetVolume, durationInS));
        }

        private IEnumerator FadeToVolumeCoroutine(IAudioSource audioSource, float targetVolume, float durationInS)
        {
            float currentTime = 0;
            float startVolume = audioSource.Volume;

            while (currentTime < durationInS)
            {
                currentTime += _time.DeltaTime;
                audioSource.Volume = Mathf.Lerp(startVolume, targetVolume, currentTime / durationInS);
                yield return null;
            }

            yield break;
        }
    }
}