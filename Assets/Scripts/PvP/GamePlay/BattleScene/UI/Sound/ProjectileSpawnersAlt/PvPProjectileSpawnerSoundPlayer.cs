using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Utils.PlatformAbstractions.Audio;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.ProjectileSpawners
{
    public abstract class PvPProjectileSpawnerSoundPlayer : IPvPProjectileSpawnerSoundPlayer
    {
        protected readonly IAudioSource _audioSource;

        protected PvPProjectileSpawnerSoundPlayer(IAudioClipWrapper audioClip, IAudioSource audioSource)
        {
            PvPHelper.AssertIsNotNull(audioClip, audioSource);

            _audioSource = audioSource;
            _audioSource.AudioClip = audioClip;
        }

        public abstract void OnProjectileFired();
    }
}
