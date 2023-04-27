using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Audio;
using System.Threading.Tasks;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.ProjectileSpawners
{
    public interface IPvPSoundPlayerFactory
    {
        IPvPProjectileSpawnerSoundPlayer DummyPlayer { get; }

        Task<IPvPProjectileSpawnerSoundPlayer> CreateShortSoundPlayerAsync(IPvPSoundKey firingSound, IPvPAudioSource audioSource);
        Task<IPvPProjectileSpawnerSoundPlayer> CreateLongSoundPlayerAsync(IPvPSoundKey firingSound, IPvPAudioSource audioSource, int burstSize, float burstEndDelayInS);
    }
}
