using BattleCruisers.Data.Models;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Items;
using BattleCruisers.Utils;
using System.Collections.Specialized;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen
{
    public class HullItemsCategoryButton : ItemCategoryButton
    {
        protected override ItemFamily ItemFamily => ItemFamily.Hulls;

        protected override bool HasNewItems(IGameModel gameModel)
        {
            return gameModel.NewHulls.Items.Count != 0;
        }

        protected override void SetupNewMarkVisibilityCallback(IGameModel gameModel)
        {
            gameModel.NewHulls.Items.Parse<INotifyCollectionChanged>().CollectionChanged += HullItemsCategoryButton_CollectionChanged;
        }

        private void HullItemsCategoryButton_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateNewItemMarkVisibility();
        }

        protected override void CleanUp(IGameModel gameModel)
        {
            gameModel.NewHulls.Items.Parse<INotifyCollectionChanged>().CollectionChanged -= HullItemsCategoryButton_CollectionChanged;
        }
    }
}