using BattleCruisers.Utils.UIWrappers;
using UnityEngine;

namespace BattleCruisers.UI.Sound
{
    public interface ISoundPlayer
    {
        void PlaySound(IAudioClipWrapper soundClip, Vector2 position);
    }
}
