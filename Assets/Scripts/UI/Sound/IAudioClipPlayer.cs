using BattleCruisers.Utils.PlatformAbstractions.UI;
using UnityEngine;

namespace BattleCruisers.UI.Sound
{
    public interface IAudioClipPlayer
    {
        void PlaySound(IAudioClipWrapper soundClip, Vector2 position);
    }
}
