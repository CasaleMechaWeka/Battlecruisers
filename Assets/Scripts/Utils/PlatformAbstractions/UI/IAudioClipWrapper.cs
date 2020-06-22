using UnityEngine;

namespace BattleCruisers.Utils.PlatformAbstractions.UI
{
    public interface IAudioClipWrapper
    {
        AudioClip AudioClip { get; }
        float Length { get; }
    }
}
