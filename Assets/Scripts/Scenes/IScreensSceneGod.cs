namespace BattleCruisers.Scenes
{
    public interface IScreensSceneGod
    {
        void GoToLevelsScreen();
        void GoToHomeScreen();
        void GoToLoadoutScreen();

        void LoadLevel(int levelNum);
    }
}
	