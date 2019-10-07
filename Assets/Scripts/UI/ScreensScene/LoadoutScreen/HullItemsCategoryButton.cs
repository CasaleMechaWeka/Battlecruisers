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
            // FELIX  TEMP
            ((INotifyCollectionChanged)gameModel.NewHulls.Items).CollectionChanged += HullItemsCategoryButton_CollectionChanged;
            gameModel.NewHulls.Items.Parse<INotifyCollectionChanged>().CollectionChanged += HullItemsCategoryButton_CollectionChanged;
            gameModel.NewHulls.Items.Parse<INotifyCollectionChanged>().CollectionChanged += (sender, e) =>
            {
                Logging.LogMethod(Tags.LOADOUT_SCREEN);
                UpdateNewItemMarkVisibility();
            };
            //gameModel.NewHulls.Items.Parse<INotifyCollectionChanged>().CollectionChanged += (sender, e) => UpdateNewItemMarkVisibility();
        }

        private void HullItemsCategoryButton_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            throw new System.NotImplementedException();
        }
    }
}