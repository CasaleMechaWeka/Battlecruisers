using BattleCruisers.Data.Settings;
using BattleCruisers.UI.Sound;
using System.Threading.Tasks;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.ProjectileSpawners
{
    public interface IPvPProjectileSoundPlayerInitialiser
    {
        Task<IPvPProjectileSpawnerSoundPlayer> CreateSoundPlayerAsync(
            IPvPSoundPlayerFactory soundPlayerFactory,
            ISoundKey firingSound,
            int burstSize,
            ISettingsManager settingsManager);
    }
}
