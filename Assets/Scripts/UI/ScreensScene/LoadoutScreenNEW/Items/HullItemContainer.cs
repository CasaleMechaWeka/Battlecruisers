using BattleCruisers.Data.Models;
using BattleCruisers.Data.Models.PrefabKeys;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.Items
{
    public class HullItemContainer : ItemContainer
    {
        public HullKey hullKey;

        protected override bool IsUnlocked(IGameModel gameModel)
        {
            return gameModel.UnlockedHulls.Contains(hullKey);
        }
    }
}