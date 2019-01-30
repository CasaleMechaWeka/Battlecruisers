using BattleCruisers.Data.Models;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Scenes.Test.Utilities;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.Items
{
    public class HullItemContainer : ItemContainer
    {
        public PrefabKeyName hullKeyName;

        protected override bool IsUnlocked(IGameModel gameModel)
        {
            HullKey hullKey = StaticPrefabKeyHelper.GetPrefabKey<HullKey>(hullKeyName);
            return gameModel.UnlockedHulls.Contains(hullKey);
        }
    }
}