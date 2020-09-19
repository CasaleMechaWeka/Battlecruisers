namespace BattleCruisers.Scenes
{
    public interface IScreensSceneGod
    {
        void GoToLevelsScreen();
        void GoToHomeScreen();
        void GoToLoadoutScreen();
        void GoToSettingsScreen();

        void LoadLevelTrashScreen(int levelNum);
        void LoadBattleScene();
    }
}
	