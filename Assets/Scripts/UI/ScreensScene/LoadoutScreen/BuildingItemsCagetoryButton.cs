using BattleCruisers.Data.Models;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Items;
using BattleCruisers.Utils;
using System.Collections.Specialized;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen
{
    public class BuildingItemsCagetoryButton : ItemCategoryButton
    {
        protected override ItemFamily ItemFamily => ItemFamily.Buildings;

        protected override bool HasNewItems(IGameModel gameModel)
        {
            return gameModel.NewBuildings.Items.Count != 0;
        }

        protected override void SetupNewMarkVisibilityCallback(IGameModel gameModel)
        {
            gameModel.NewBuildings.Items.Parse<INotifyCollectionChanged>().CollectionChanged += (sender, e) => UpdateNewItemMarkVisibility();
        }
    }
}