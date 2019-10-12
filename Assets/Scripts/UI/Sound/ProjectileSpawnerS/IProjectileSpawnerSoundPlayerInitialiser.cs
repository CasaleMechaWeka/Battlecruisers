using System.Threading.Tasks;

namespace BattleCruisers.UI.Sound.ProjectileSpawners
{
    public interface IProjectileSoundPlayerInitialiser
    {
        Task<IProjectileSpawnerSoundPlayer> CreateSoundPlayerAsync(ISoundPlayerFactory soundPlayerFactory, ISoundKey firingSound, int burstSize);
    }
}
