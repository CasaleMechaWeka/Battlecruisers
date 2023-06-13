using BattleCruisers.Data.Models;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Items;
using BattleCruisers.Utils;
using System.Collections.Specialized;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen
{
    //public class CaptainsItemsCategoryButton : ItemCategoryButton
    //{
    //    protected override ItemFamily ItemFamily => ItemFamily.Captains;

    //    protected override bool HasNewItems(IGameModel gameModel)
    //    {
    //        return gameModel.NewCaptains.Items.Count != 0;
    //    }

    //    protected override void SetupNewMarkVisibilityCallback(IGameModel gameModel)
    //    {
    //        gameModel.NewCaptains.Items.Parse<INotifyCollectionChanged>().CollectionChanged += NewCaptainsCategoryButton_CollectionChanged;
    //    }

    //    private void NewCaptainsCategoryButton_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    //    {
    //        UpdateNewItemMarkVisibility();
    //    }

    //    protected override void CleanUp(IGameModel gameModel)
    //    {
    //        gameModel.NewCaptains.Items.Parse<INotifyCollectionChanged>().CollectionChanged -= NewCaptainsCategoryButton_CollectionChanged;
    //    }
    //}
}
