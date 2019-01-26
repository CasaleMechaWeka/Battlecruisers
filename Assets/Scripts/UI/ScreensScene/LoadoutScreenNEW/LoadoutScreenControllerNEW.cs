using BattleCruisers.Cruisers;
using BattleCruisers.Data;
using BattleCruisers.Scenes;
using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.ItemDetails;
using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.Items;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreenNEW
{
    // FELIX  Delete all old classes :D
    // FELIX  Remove NEW from all file and class names :)
    public class LoadoutScreenControllerNEW : ScreenController
    {
        private IDataProvider _dataProvider;
        private IPrefabFactory _prefabFactory;
        private IItemDetailsDisplayer _itemDetailsDisplayer;

        public void Initialise(
            IScreensSceneGod screensSceneGod, 
            IDataProvider dataProvider,
            IPrefabFactory prefabFactory)
        {
            base.Initialise(screensSceneGod);

            Helper.AssertIsNotNull(dataProvider, prefabFactory);

            _dataProvider = dataProvider;
            _prefabFactory = prefabFactory;

            // FELIX  Move some initialisation down?  Into child game objects?
            ItemDetailsPanel itemDetailsPanel = GetComponentInChildren<ItemDetailsPanel>(includeInactive: true);
            Assert.IsNotNull(itemDetailsPanel);
            itemDetailsPanel.Initialise();

            _itemDetailsDisplayer = new ItemDetailsDisplayer(itemDetailsPanel);

            ItemPanelsController itemPanels = GetComponentInChildren<ItemPanelsController>(includeInactive: true);
            Assert.IsNotNull(itemPanels);
            itemPanels.Initialise(_itemDetailsDisplayer, ItemType.Hull);

            CategoryButtonsPanel categoryButtonsPanel = GetComponentInChildren<CategoryButtonsPanel>(includeInactive: true);
            Assert.IsNotNull(categoryButtonsPanel);
            categoryButtonsPanel.Initialise(itemPanels);

            ShowPlayerHull();
        }

        private void ShowPlayerHull()
        {
            ICruiser playerCruiser = _prefabFactory.GetCruiserPrefab(_dataProvider.GameModel.PlayerLoadout.Hull);
            _itemDetailsDisplayer.ShowDetails(playerCruiser);
        }

        public void Cancel()
        {
            _screensSceneGod.GoToHomeScreen();
        }

        public void Save()
        {
            // FELIX  Save hull choice :)
            _screensSceneGod.GoToHomeScreen();
        }
    }
}