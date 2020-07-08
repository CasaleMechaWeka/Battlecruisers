using UnityEngine;

namespace BattleCruisers.Utils.PlatformAbstractions.Audio
{
    public interface IAudioClipWrapper
    {
        AudioClip AudioClip { get; }
        float Length { get; }
    }
}
