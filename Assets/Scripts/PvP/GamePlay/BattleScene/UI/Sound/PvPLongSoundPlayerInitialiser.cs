using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Audio;
using System.Threading.Tasks;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.ProjectileSpawners
{
    public class PvPLongSoundPlayerInitialiser : PvPProjectileSoundPlayerInitialiser
    {
        public float burstEndDelayInS;

        protected override async Task<IPvPProjectileSpawnerSoundPlayer> CreateSoundPlayerAsync(
            IPvPSoundPlayerFactory soundPlayerFactory,
            IPvPSoundKey firingSound,
            int burstSize,
            IPvPAudioSource audioSource)
        {
            return await soundPlayerFactory.CreateLongSoundPlayerAsync(firingSound, audioSource, burstSize, burstEndDelayInS);
        }
    }
}
