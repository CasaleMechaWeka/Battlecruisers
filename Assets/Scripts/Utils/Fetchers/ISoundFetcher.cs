using BattleCruisers.Utils.UIWrappers;

namespace BattleCruisers.Fetchers
{
    public interface ISoundFetcher
    {
        IAudioClipWrapper GetSound(string soundName);
    }
}
