using BattleCruisers.UI.Sound;
using BattleCruisers.UI.Sound.ProjectileSpawners;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using System.Threading.Tasks;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.ProjectileSpawners
{
    public class PvPLongSoundPlayerInitialiser : ProjectileSoundPlayerInitialiser
    {
        public float burstEndDelayInS;

        protected override async Task<IProjectileSpawnerSoundPlayer> CreateSoundPlayerAsync(
            ISoundPlayerFactory soundPlayerFactory,
            ISoundKey firingSound,
            int burstSize,
            IAudioSource audioSource)
        {
            return await soundPlayerFactory.CreateLongSoundPlayerAsync(firingSound, audioSource, burstSize, burstEndDelayInS);
        }
    }
}
