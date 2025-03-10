using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace BattleCruisers.Utils.PlatformAbstractions.Audio
{
    public interface IAudioClipWrapper
    {
        AudioClip AudioClip { get; }
        float Length { get; }
        AsyncOperationHandle<AudioClip> Handle { get; }
    }
}
