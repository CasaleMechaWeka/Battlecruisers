using BattleCruisers.Data.Models;
using BattleCruisers.UI.ScreensScene.HomeScreen.Buttons;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.ScreensScene.HomeScreen
{
    public class HomeScreenLayout : MonoBehaviourWrapper
    {
        public void Initialise(IHomeScreen homeScreen, IGameModel gameModel)
        {
            Helper.AssertIsNotNull(homeScreen, gameModel);

            HomeScreenButton[] buttons = GetComponentsInChildren<HomeScreenButton>();

            foreach (HomeScreenButton button in buttons)
            {
                button.Initialise(homeScreen, gameModel);
            }
       }
    }
}