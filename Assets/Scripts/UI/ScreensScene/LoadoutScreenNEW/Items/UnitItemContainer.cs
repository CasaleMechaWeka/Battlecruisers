using BattleCruisers.Data.Models;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.Items
{
    public class UnitItemContainer : ItemContainer
    {
        public PrefabKeyName unitKeyName;

        protected override bool IsUnlocked(IGameModel gameModel)
        {
            UnitKey unitKey = StaticPrefabKeyHelper.GetPrefabKey<UnitKey>(unitKeyName);
            return gameModel.UnlockedUnits.Contains(unitKey);
        }
    }
}