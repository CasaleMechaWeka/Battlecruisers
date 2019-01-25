using BattleCruisers.Data;
using BattleCruisers.Scenes;
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

            ItemPanelsController itemPanels = GetComponentInChildren<ItemPanelsController>(includeInactive: true);
            Assert.IsNotNull(itemPanels);
            itemPanels.Initialise(ItemType.Hull);
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