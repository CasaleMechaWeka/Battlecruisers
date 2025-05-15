using BattleCruisers.Data.Settings;
using System.Threading.Tasks;

namespace BattleCruisers.UI.Sound.ProjectileSpawners
{
    public interface IProjectileSoundPlayerInitialiser
    {
        Task<IProjectileSpawnerSoundPlayer> CreateSoundPlayerAsync(
            ISoundPlayerFactory soundPlayerFactory,
            SoundKey firingSound,
            int burstSize,
            SettingsManager settingsManager);
    }
}
