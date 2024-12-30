using BattleCruisers.UI.Sound;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using System.Threading.Tasks;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers
{
    public interface IPvPSoundFetcher
    {
        Task<IAudioClipWrapper> GetSoundAsync(ISoundKey soundKey);
    }
}
