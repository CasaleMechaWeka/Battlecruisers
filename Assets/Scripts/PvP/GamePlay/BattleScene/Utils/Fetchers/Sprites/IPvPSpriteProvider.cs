using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.UI;
using BattleCruisers.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers.Sprites
{
    public interface IPvPSpriteProvider
    {
        Task<IList<IPvPSpriteWrapper>> GetAircraftSpritesAsync(PrefabKeyName prefabKeyName);
    }
}
