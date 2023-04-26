using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Audio
{
    public interface IPvPAudioSource
    {
        bool IsPlaying { get; }
        IPvPAudioClipWrapper AudioClip { set; }
        float Volume { get; set; }
        Vector2 Position { get; set; }
        bool IsActive { get; set; }

        /// <param name="isSpatial">
        /// True if the sound should get quieter the further away from the camera it is.
        /// </param>
        /// <param name="loop">
        /// True if the sound should loop (eg: engine), false otherwise.
        /// </param>
        void Play(bool isSpatial = true, bool loop = false);

        void Stop();
        void FreeAudioClip();
    }
}