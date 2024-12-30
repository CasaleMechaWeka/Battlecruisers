using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Audio;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Audio
{
    public interface IPvPAudioVolumeFade
    {
        void FadeToVolume(IPvPAudioSource audioSource, float targetVolume, float durationInS);

        /// <summary>
        /// For cutting the fade short.  Useful when the user has changed the volume, and we
        /// want to instantly go to their new desired volume.  Ok to call if not during fade.
        /// </summary>
        void Stop();
    }
}