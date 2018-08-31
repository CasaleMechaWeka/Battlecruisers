using System.Collections;

// FELIX  Move all sound to own namespace :P (not in UI)
namespace BattleCruisers.Utils.PlatformAbstractions.UI
{
    public interface IAwaitableAudioSource
    {
        IEnumerator Play(IAudioClipWrapper audioClip);
    }
}