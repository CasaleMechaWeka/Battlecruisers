using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound.Players;

namespace BattleCruisers.UI.ScreensScene.HomeScreen.Buttons
{
    public class SkirmishButton : HomeScreenButton
    {
        public override void Initialise(ISingleSoundPlayer soundPlayer, IHomeScreen homeScreen, IGameModel gameModel)
        {
            base.Initialise(soundPlayer, homeScreen, gameModel);

            // Only enable skirmish screen once user has completed campaign
            if (gameModel.NumOfLevelsCompleted != StaticData.NUM_OF_LEVELS
                // TEMP  Disable skirmish on release builds, until big update announcement
                || !ApplicationModelProvider.ApplicationModel.DataProvider.StaticData.HasAsserts)
            {
                DestroySelf();
            }
        }

        protected override void OnClicked()
        {
            base.OnClicked();
            _homeScreen.GoToSkirmishScreen();
        }
    }
}