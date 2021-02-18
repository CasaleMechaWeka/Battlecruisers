namespace BattleCruisers.Scenes
{
    public interface IScreensSceneGod
    {
        void GoToLevelsScreen();
        void GoToHomeScreen();
        void GoToLoadoutScreen();
        void GoToSettingsScreen();
        void GoToChooseDifficultyScreen();
        void GoToSkirmishScreen();

        void GoToTrashScreen(int levelNum);
        void LoadBattleScene();
        void LoadCreditsScene();
    }
}
	