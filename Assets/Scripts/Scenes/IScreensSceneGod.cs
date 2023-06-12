using BattleCruisers.UI.ScreensScene;
namespace BattleCruisers.Scenes
{
    public interface IScreensSceneGod
    {
        void GoToLevelsScreen();
        void GoToHomeScreen();
        void GoToLoadoutScreen();
        void GotoHubScreen();
        void GoToSettingsScreen();
        void GoToChooseDifficultyScreen();
        void GoToSkirmishScreen();
        void GoToTrashScreen(int levelNum);
        void GoStraightToTrashScreen(int levelNum);
        void LoadBattleScene();
        void LoadCreditsScene();
        void LoadCutsceneScene();
        void LoadPvPBattleScene();
        void LoadBattle1v1Mode();
    }
}
