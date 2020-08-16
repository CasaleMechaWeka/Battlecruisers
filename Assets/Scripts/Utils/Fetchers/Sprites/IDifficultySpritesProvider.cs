using BattleCruisers.Data.Settings;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using System.Threading.Tasks;

namespace BattleCruisers.Utils.Fetchers.Sprites
{
    public interface IDifficultySpritesProvider
    {
        Task<ISpriteWrapper> GetSpriteAsync(Difficulty difficulty);
    }
}