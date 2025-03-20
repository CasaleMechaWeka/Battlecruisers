using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace BattleCruisers.Utils.Fetchers.Sprites
{
    public interface ISpriteProvider
    {
        Task<IList<Sprite>> GetAircraftSpritesAsync(PrefabKeyName prefabKeyName);
    }
}
