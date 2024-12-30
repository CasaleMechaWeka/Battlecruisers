using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Audio;
using BattleCruisers.UI.Sound;
using System.Threading.Tasks;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.ProjectileSpawners
{
    public class PvPShortSoundPlayerInitialiser : PvPProjectileSoundPlayerInitialiser
    {
        protected override async Task<IPvPProjectileSpawnerSoundPlayer> CreateSoundPlayerAsync(
            IPvPSoundPlayerFactory soundPlayerFactory,
            ISoundKey firingSound,
            int burstSize,
            IPvPAudioSource audioSource)
        {
            return await soundPlayerFactory.CreateShortSoundPlayerAsync(firingSound, audioSource);
        }
    }
}
