using BattleCruisers.Data;
using BattleCruisers.Scenes;
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

            // FELIX  Move down, into MainPanelController?
            ItemPanelsController itemPanels = GetComponentInChildren<ItemPanelsController>(includeInactive: true);
            Assert.IsNotNull(itemPanels);
            itemPanels.Initialise(ItemType.Hull);

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