using BattleCruisers.UI.Sound;
using BattleCruisers.Utils.UIWrappers;

namespace BattleCruisers.Utils.Fetchers
{
    public interface ISoundFetcher
    {
        IAudioClipWrapper GetSound(ISoundKey soundKey);
    }
}
