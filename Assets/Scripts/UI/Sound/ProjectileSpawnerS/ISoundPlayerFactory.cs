using BattleCruisers.Utils.PlatformAbstractions.Audio;
using System.Threading.Tasks;

namespace BattleCruisers.UI.Sound.ProjectileSpawners
{
    public interface ISoundPlayerFactory
    {
        IProjectileSpawnerSoundPlayer DummyPlayer { get; }

        Task<IProjectileSpawnerSoundPlayer> CreateShortSoundPlayerAsync(ISoundKey firingSound, IAudioSource audioSource);
        Task<IProjectileSpawnerSoundPlayer> CreateLongSoundPlayerAsync(ISoundKey firingSound, IAudioSource audioSource, int burstSize, float burstEndDelayInS);
    }
}
