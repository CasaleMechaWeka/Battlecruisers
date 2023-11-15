using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Audio;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Threading;
using UnityEngine.Assertions;


namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.ProjectileSpawners
{
    /// <summary>
    /// Plays the projectile firing only at the start of the burst.  Stops playing the sound after
    /// the specified delay after the last shot in the burst.  For long sounds, such as the anti-air
    /// turret sound.
    /// </summary>
    public class PvPLongSoundPlayer : PvPProjectileSpawnerSoundPlayer
    {
        private readonly int _burstSize;
        private readonly IPvPDeferrer _deferrer;
        private readonly float _burstEndDelayInS;
        private int _burstIndex;

        private const int MIN_BURST_SIZE = 2;

        public PvPLongSoundPlayer(
            IPvPAudioClipWrapper audioClip,
            IPvPAudioSource audioSource,
            IPvPDeferrer deferrer,
            int burstSize,
            float burstEndDelayInS)
            : base(audioClip, audioSource)
        {
            PvPHelper.AssertIsNotNull(deferrer);
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
