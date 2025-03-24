using BattleCruisers.Utils.PlatformAbstractions.Audio;
using UnityEngine;

namespace BattleCruisers.UI.Sound.Players
{
    public interface IAudioClipPlayer
    {
        void PlaySound(AudioClipWrapper soundClip, Vector3 position);
    }
}
