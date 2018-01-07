using BattleCruisers.Utils.UIWrappers;

namespace BattleCruisers.Utils.Fetchers
{
    public interface ISoundFetcher
    {
        IAudioClipWrapper GetSound(string soundName);
    }
}
