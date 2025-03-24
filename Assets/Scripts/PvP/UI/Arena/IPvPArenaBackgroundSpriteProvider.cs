using System.Threading.Tasks;
using BattleCruisers.Network.Multiplay.Matchplay.Shared;
using UnityEngine;


namespace BattleCruisers.Utils.Fetchers.Sprites
{
    public interface IPvPArenaBackgroundSpriteProvider
    {
        // Start is called before the first frame update
        Task<Sprite> GetSpriteAsync(Map map);
    }
}

