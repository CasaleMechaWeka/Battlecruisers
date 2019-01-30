using BattleCruisers.Data.Models;
using BattleCruisers.Data.Models.PrefabKeys;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.Items
{
    public class UnitItemContainer : ItemContainer
    {
        public UnitKey unitKey;

        protected override bool IsUnlocked(IGameModel gameModel)
        {
            return gameModel.UnlockedUnits.Contains(unitKey);
        }
    }
}