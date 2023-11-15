using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Audio;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.ProjectileSpawners
{
    public abstract class PvPProjectileSpawnerSoundPlayer : IPvPProjectileSpawnerSoundPlayer
    {
        protected readonly IPvPAudioSource _audioSource;

        protected PvPProjectileSpawnerSoundPlayer(IPvPAudioClipWrapper audioClip, IPvPAudioSource audioSource)
        {
            PvPHelper.AssertIsNotNull(audioClip, audioSource);

            _audioSource = audioSource;
            _audioSource.AudioClip = audioClip;
        }

        public abstract void OnProjectileFired();
    }
}
