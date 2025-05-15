using BattleCruisers.Utils.PlatformAbstractions.Audio;
using System.Threading.Tasks;

namespace BattleCruisers.UI.Sound.ProjectileSpawners
{
    public interface ISoundPlayerFactory
    {
        IProjectileSpawnerSoundPlayer DummyPlayer { get; }

        Task<IProjectileSpawnerSoundPlayer> CreateShortSoundPlayerAsync(SoundKey firingSound, IAudioSource audioSource);
        Task<IProjectileSpawnerSoundPlayer> CreateLongSoundPlayerAsync(SoundKey firingSound, IAudioSource audioSource, int burstSize, float burstEndDelayInS);
    }
}
