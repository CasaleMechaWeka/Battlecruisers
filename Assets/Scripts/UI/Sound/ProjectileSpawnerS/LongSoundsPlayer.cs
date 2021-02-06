using BattleCruisers.Data.Settings;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using BattleCruisers.Utils.Threading;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Sound.ProjectileSpawners
{
    /// <summary>
    /// Plays the projectile firing only at the start of the burst.  Stops playing the sound after
    /// the specified delay after the last shot in the burst.  For long sounds, such as the anti-air
    /// turret sound.
    /// </summary>
    public class LongSoundPlayer : ProjectileSpawnerSoundPlayer
    {
        private readonly int _burstSize;
        private readonly IDeferrer _deferrer;
        private readonly float _burstEndDelayInS;
        private int _burstIndex;

        private const int MIN_BURST_SIZE = 2;

        public LongSoundPlayer(
            IAudioClipWrapper audioClip, 
            IAudioSource audioSource,
            ISettingsManager settingsManager,
            IDeferrer deferrer, 
            int burstSize, 
            float burstEndDelayInS)
            : base(audioClip, audioSource, settingsManager)
        {
            Helper.AssertIsNotNull(deferrer);
            Assert.IsTrue(burstSize >= MIN_BURST_SIZE, $"burstSize: {burstSize}  MIN_BURST_SIZE: {MIN_BURST_SIZE}");

            _deferrer = deferrer;
            _burstSize = burstSize;
            _burstEndDelayInS = burstEndDelayInS;
            _burstIndex = 0;
        }

        public override void OnProjectileFired()
        {
            _burstIndex++;

            if (_burstIndex == 1)
            {
                OnBurstStart();
            }
            else if (_burstIndex == _burstSize)
            {
                OnBurstEnd();
            }
        }

        private void OnBurstStart()
        {
            _audioSource.Play();
        }

        private void OnBurstEnd()
        {
            _burstIndex = 0;
            _deferrer.Defer(() => _audioSource.Stop(), _burstEndDelayInS);
        }
    }
}
