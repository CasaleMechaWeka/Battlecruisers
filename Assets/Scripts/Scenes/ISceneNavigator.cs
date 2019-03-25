namespace BattleCruisers.Scenes
{
    public interface ISceneNavigator
    {
        void GoToScene(string sceneName, string loadingScreenHint = null);
    }
}
