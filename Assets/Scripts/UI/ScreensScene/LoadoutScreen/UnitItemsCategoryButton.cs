using BattleCruisers.Data.Models;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Items;
using BattleCruisers.Utils;
using System.Collections.Specialized;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen
{
    public class UnitItemsCategoryButton : ItemCategoryButton
    {
        protected override ItemFamily ItemFamily => ItemFamily.Units;

        protected override bool HasNewItems(IGameModel gameModel)
        {
            return gameModel.NewUnits.Items.Count != 0;
        }

        protected override void SetupNewMarkVisibilityCallback(IGameModel gameModel)
        {
            gameModel.NewUnits.Items.Parse<INotifyCollectionChanged>().CollectionChanged += (sender, e) => UpdateNewItemMarkVisibility();
        }
    }
}