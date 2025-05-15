using BattleCruisers.Data.Models;
using BattleCruisers.UI.ScreensScene.HomeScreen.Buttons;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.ScreensScene.HomeScreen
{
    public class HomeScreenLayout : MonoBehaviourWrapper
    {
        public void Initialise(IHomeScreen homeScreen, GameModel gameModel, SingleSoundPlayer soundPlayer)
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