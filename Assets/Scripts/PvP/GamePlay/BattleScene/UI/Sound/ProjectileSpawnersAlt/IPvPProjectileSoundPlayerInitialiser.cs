using BattleCruisers.Data.Settings;
using BattleCruisers.UI.Sound;
using BattleCruisers.UI.Sound.ProjectileSpawners;
using System.Threading.Tasks;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.ProjectileSpawners
{
    public interface IPvPProjectileSoundPlayerInitialiser
    {
        Task<IProjectileSpawnerSoundPlayer> CreateSoundPlayerAsync(
            ISoundPlayerFactory soundPlayerFactory,
            ISoundKey firingSound,
            int burstSize,
            ISettingsManager settingsManager);
    }
}
