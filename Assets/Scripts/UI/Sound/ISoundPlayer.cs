using BattleCruisers.Utils.PlatformAbstractions.UI;
using System.Threading.Tasks;
using UnityEngine;

namespace BattleCruisers.UI.Sound
{
    public interface ISoundPlayer
    {
        Task PlaySoundAsync(ISoundKey soundKey);
        Task PlaySoundAsync(ISoundKey soundKey, Vector2 position);
        void PlaySound(IAudioClipWrapper sound, Vector2 position);
    }
}
