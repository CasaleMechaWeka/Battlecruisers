using System;

// FELIX  Move all sound to own namespace :P (not in UI)
namespace BattleCruisers.Utils.PlatformAbstractions.UI
{
    public interface IAwaitableAudioSource
    {
        void Play(IAudioClipWrapper audioClip, Action audioCompletedCallback);
    }
}