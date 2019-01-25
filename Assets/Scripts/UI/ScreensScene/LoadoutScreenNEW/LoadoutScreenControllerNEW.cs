using BattleCruisers.Data;
using BattleCruisers.Scenes;
using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.ItemDetails;
using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.Items;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreenNEW
{
    // FELIX  Delete all old classes :D
    // FELIX  Remove NEW from all file and class names :)
    public class LoadoutScreenControllerNEW : ScreenController
    {
        private IDataProvider _dataProvider;

        public void Initialise(IScreensSceneGod screensSceneGod, IDataProvider dataProvider)
        {
            base.Initialise(screensSceneGod);

            Assert.IsNotNull(dataProvider);
            _dataProvider = dataProvider;

            // FELIX  Move some initialisation down?  Into child game objects?
            ItemDetailsPanel itemDetailsPanel = GetComponentInChildren<ItemDetailsPanel>(includeInactive: true);
            Assert.IsNotNull(itemDetailsPanel);
            itemDetailsPanel.Initialise();

            ItemPanelsController itemPanels = GetComponentInChildren<ItemPanelsController>(includeInactive: true);
            Assert.IsNotNull(itemPanels);
            itemPanels.Initialise(new ItemDetailsDisplayer(itemDetailsPanel), ItemType.Hull);

            CategoryButtonsPanel categoryButtonsPanel = GetComponentInChildren<CategoryButtonsPanel>(includeInactive: true);
            Assert.IsNotNull(categoryButtonsPanel);
            categoryButtonsPanel.Initialise(itemPanels);
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