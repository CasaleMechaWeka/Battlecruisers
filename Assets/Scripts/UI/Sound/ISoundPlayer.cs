using BattleCruisers.Utils.PlatformAbstractions.UI;
using UnityEngine;

namespace BattleCruisers.UI.Sound
{
    public interface ISoundPlayer
    {
        void PlaySound(IAudioClipWrapper soundClip, Vector2 position);
    }
}
