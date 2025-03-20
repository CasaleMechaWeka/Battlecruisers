using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace BattleCruisers.Utils.Fetchers.Sprites
{
    public interface ISpriteFetcher
    {
        Task<Sprite> GetSpriteAsync(string spritePath);
        Task<IList<Sprite>> GetMultiSpritesAsync(string spritePath);
    }
}
