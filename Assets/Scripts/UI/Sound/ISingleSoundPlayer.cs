using BattleCruisers.Utils.PlatformAbstractions.UI;
using System.Threading.Tasks;

namespace BattleCruisers.UI.Sound
{
    public interface ISingleSoundPlayer
    {
        bool IsPlayingSound { get; }

        Task PlaySoundAsync(ISoundKey soundKey, bool loop = false);
        void PlaySound(IAudioClipWrapper sound, bool loop = false);

        void Stop();
    }
}
