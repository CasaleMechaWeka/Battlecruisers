using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Audio;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Threading;
using System.Collections;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Time;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Audio
{
    public class PvPAudioVolumeFade : IPvPAudioVolumeFade
    {
        private readonly IPvPCoroutineStarter _coroutineStarter;
        private readonly IPvPTime _time;
        private bool _shouldFade;

        public PvPAudioVolumeFade(IPvPCoroutineStarter coroutineStarter, IPvPTime time)
        {
            PvPHelper.AssertIsNotNull(coroutineStarter, time);

            _coroutineStarter = coroutineStarter;
            _time = time;
            _shouldFade = false;
        }

        public void FadeToVolume(IPvPAudioSource audioSource, float targetVolume, float durationInS)
        {
            Assert.IsNotNull(audioSource);

            _coroutineStarter.StartRoutine(FadeToVolumeCoroutine(audioSource, targetVolume, durationInS));
        }

        private IEnumerator FadeToVolumeCoroutine(IPvPAudioSource audioSource, float targetVolume, float durationInS)
        {
            _shouldFade = true;

            float currentTime = 0;
            float startVolume = audioSource.Volume;

            while (currentTime < durationInS
                && _shouldFade)
            {
                currentTime += _time.DeltaTime;
                audioSource.Volume = Mathf.Lerp(startVolume, targetVolume, currentTime / durationInS);
                yield return null;
            }

            yield break;
        }

        public void Stop()
        {
            _shouldFade = false;
        }
    }
}