using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.UI.Sound.ProjectileSpawners;
using BattleCruisers.Utils.PlatformAbstractions.Audio;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.ProjectileSpawners
{
    /// <summary>
    /// Plays the projectile firing sound every time a projectile is fired.  For short sounds.
    /// </summary>
    public class PvPShortSoundPlayer : ProjectileSpawnerSoundPlayer
    {
        public PvPShortSoundPlayer(IAudioClipWrapper audioClip, IAudioSource audioSource)
            : base(audioClip, audioSource)
        {
            PvPHelper.AssertIsNotNull(audioClip, audioSource);
        }

        public override void OnProjectileFired()
        {
            _audioSource.Play();
        }
    }
}
