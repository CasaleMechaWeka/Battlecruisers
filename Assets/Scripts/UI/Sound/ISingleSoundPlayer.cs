using BattleCruisers.Utils.PlatformAbstractions.Audio;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace BattleCruisers.UI.Sound
{
    public interface ISingleSoundPlayer
    {
        bool IsPlayingSound { get; }
        float Volume { set; }

        Task<AsyncOperationHandle<AudioClip>> PlaySoundAsync(ISoundKey soundKey, bool loop = false);
        void PlaySound(IAudioClipWrapper sound, bool loop = false);

        void Stop();
    }
}
