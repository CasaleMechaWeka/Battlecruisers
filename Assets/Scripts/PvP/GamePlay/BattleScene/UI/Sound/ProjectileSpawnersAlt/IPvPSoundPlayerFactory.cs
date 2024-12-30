using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Audio;
using BattleCruisers.UI.Sound;
using System.Threading.Tasks;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.ProjectileSpawners
{
    public interface IPvPSoundPlayerFactory
    {
        IPvPProjectileSpawnerSoundPlayer DummyPlayer { get; }

        Task<IPvPProjectileSpawnerSoundPlayer> CreateShortSoundPlayerAsync(ISoundKey firingSound, IPvPAudioSource audioSource);
        Task<IPvPProjectileSpawnerSoundPlayer> CreateLongSoundPlayerAsync(ISoundKey firingSound, IPvPAudioSource audioSource, int burstSize, float burstEndDelayInS);
    }
}
