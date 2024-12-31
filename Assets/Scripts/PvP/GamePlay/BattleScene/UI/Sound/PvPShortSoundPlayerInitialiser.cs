using BattleCruisers.UI.Sound;
using BattleCruisers.UI.Sound.ProjectileSpawners;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using System.Threading.Tasks;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.ProjectileSpawners
{
    public class PvPShortSoundPlayerInitialiser : ProjectileSoundPlayerInitialiser
    {
        protected override async Task<IProjectileSpawnerSoundPlayer> CreateSoundPlayerAsync(
            ISoundPlayerFactory soundPlayerFactory,
            ISoundKey firingSound,
            int burstSize,
            IAudioSource audioSource)
        {
            return await soundPlayerFactory.CreateShortSoundPlayerAsync(firingSound, audioSource);
        }
    }
}
