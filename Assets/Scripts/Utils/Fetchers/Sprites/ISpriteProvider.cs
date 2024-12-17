using BattleCruisers.Utils.PlatformAbstractions.UI;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BattleCruisers.Utils.Fetchers.Sprites
{
    public interface ISpriteProvider
    {
        Task<IList<ISpriteWrapper>> GetAircraftSpritesAsync(PrefabKeyName prefabKeyName);
    }
}
