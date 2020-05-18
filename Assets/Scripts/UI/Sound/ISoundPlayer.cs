using BattleCruisers.Utils.PlatformAbstractions.UI;
using System.Threading.Tasks;
using UnityEngine;

namespace BattleCruisers.UI.Sound
{
    public interface ISoundPlayer
    {
        /// <summary>
        /// Play sound at the current audio listener position, for full volume.
        /// Note this breaks if the camera is moved after the sound  has started playing.
        /// </summary>
        Task PlaySoundAsync(ISoundKey soundKey);
        Task PlaySoundAsync(ISoundKey soundKey, Vector2 position);
        /// <summary>
        /// Play sound at the current audio listener position, for full volume.
        /// Note this breaks if the camera is moved after the sound  has started playing.
        /// </summary>
        void PlaySound(IAudioClipWrapper sound);
        void PlaySound(IAudioClipWrapper sound, Vector2 position);
    }
}
