using BattleCruisers.Data.Models;
using BattleCruisers.UI.ScreensScene.HomeScreen.Buttons;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.ScreensScene.HomeScreen
{
    public class HomeScreenLayout : MonoBehaviourWrapper
    {
        public void Initialise(IHomeScreen homeScreen, IGameModel gameModel, ISoundPlayer soundPlayer)
        {
            Helper.AssertIsNotNull(homeScreen, gameModel, soundPlayer);

            HomeScreenButton[] buttons = GetComponentsInChildren<HomeScreenButton>();

            foreach (HomeScreenButton button in buttons)
            {
                button.Initialise(soundPlayer, homeScreen, gameModel);
            }
       }
    }
}