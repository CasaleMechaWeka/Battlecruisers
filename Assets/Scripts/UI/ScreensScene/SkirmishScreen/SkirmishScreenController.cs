using BattleCruisers.Data;
using BattleCruisers.Data.Settings;
using BattleCruisers.Data.Skirmishes;
using BattleCruisers.Data.Static;
using BattleCruisers.Data.Static.Strategies;
using BattleCruisers.Scenes;
using BattleCruisers.UI.ScreensScene.SettingsScreen;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.ScreensScene.SkirmishScreen
{
    public class SkirmishScreenController : ScreenController
    {
        private IApplicationModel _applicationModel;

        public CanvasGroupButton battleButton, homeButton;
        public DifficultyDropdown difficultyDropdown;

        public void Initialise(
            IScreensSceneGod screensSceneGod, 
            IApplicationModel applicationModel,
            ISingleSoundPlayer soundPlayer)
        {
            base.Initialise(screensSceneGod);

            Helper.AssertIsNotNull(battleButton, homeButton, difficultyDropdown);
            Helper.AssertIsNotNull(applicationModel, soundPlayer);

            _applicationModel = applicationModel;

            battleButton.Initialise(soundPlayer, Battle, this);
            homeButton.Initialise(soundPlayer, Home, this);
            difficultyDropdown.Initialise(applicationModel.DataProvider.GameModel.Settings.AIDifficulty);
        }

        public void Battle()
        {
            _applicationModel.Mode = GameMode.Skirmish;
            // FELIX  Get values from UI :)
            _applicationModel.Skirmish
                = new Skirmish(
                    difficultyDropdown.Difficulty,
                    StaticPrefabKeys.Hulls.Megalodon,
                    StrategyType.Rush);
            _screensSceneGod.LoadBattleScene();
        }

        public void Home()
        {
            _screensSceneGod.GoToHomeScreen();
        }

        public override void Cancel()
        {
            Home();
        }
    }
}