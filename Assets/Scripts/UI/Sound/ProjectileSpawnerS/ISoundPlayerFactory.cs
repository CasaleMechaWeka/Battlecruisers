using BattleCruisers.Utils.PlatformAbstractions.UI;
using System.Threading.Tasks;

namespace BattleCruisers.UI.Sound.ProjectileSpawners
{
    public interface ISoundPlayerFactory
    {
        Task<IProjectileSpawnerSoundPlayer> CreateShortSoundPlayerAsync(ISoundKey firingSound, IAudioSource audioSource);
        Task<IProjectileSpawnerSoundPlayer> CreateLongSoundPlayerAsync(ISoundKey firingSound, IAudioSource audioSource, int burstSize, float burstEndDelayInS);
    }
}
