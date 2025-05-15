using BattleCruisers.Utils.PlatformAbstractions.Audio;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace BattleCruisers.UI.Sound.Players
{
    public interface ISingleSoundPlayer
    {
        bool IsPlayingSound { get; }

        Task<AsyncOperationHandle<AudioClip>> PlaySoundAsync(SoundKey soundKey, bool loop = false);
        void PlaySound(AudioClipWrapper sound, bool loop = false);
        void Stop();
    }
}
