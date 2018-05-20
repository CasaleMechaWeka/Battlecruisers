using BattleCruisers.UI.Sound;
using BattleCruisers.Utils.PlatformAbstractions.UI;

namespace BattleCruisers.Utils.Fetchers
{
    public interface ISoundFetcher
    {
        IAudioClipWrapper GetSound(ISoundKey soundKey);
    }
}
