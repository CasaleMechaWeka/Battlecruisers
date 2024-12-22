using BattleCruisers.Data.Settings;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using System.Threading.Tasks;
using BattleCruisers.Network.Multiplay.Matchplay.Shared;


namespace BattleCruisers.Utils.Fetchers.Sprites
{
    public interface IPvPArenaBackgroundSpriteProvider
    {
        // Start is called before the first frame update
        Task<ISpriteWrapper> GetSpriteAsync(Map map);
    }
}

