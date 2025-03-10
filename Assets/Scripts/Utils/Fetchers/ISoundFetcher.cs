using BattleCruisers.UI.Sound;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using System.Threading.Tasks;

namespace BattleCruisers.Utils.Fetchers
{
    public interface ISoundFetcher
    {
        Task<IAudioClipWrapper> GetSoundAsync(ISoundKey soundKey);
    }
}
