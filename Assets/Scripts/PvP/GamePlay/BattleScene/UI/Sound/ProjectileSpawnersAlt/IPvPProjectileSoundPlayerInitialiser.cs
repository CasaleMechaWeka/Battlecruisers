using BattleCruisers.Data.Settings;
using System.Threading.Tasks;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.ProjectileSpawners
{
    public interface IPvPProjectileSoundPlayerInitialiser
    {
        Task<IPvPProjectileSpawnerSoundPlayer> CreateSoundPlayerAsync(
            IPvPSoundPlayerFactory soundPlayerFactory,
            IPvPSoundKey firingSound,
            int burstSize,
            ISettingsManager settingsManager);
    }
}
