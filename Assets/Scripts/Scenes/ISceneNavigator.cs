namespace BattleCruisers.Scenes
{
    /// <summary>
    /// SceneManager.LoadSceneAsync completes once all game object Start() methods have completed
    /// (or have reached an await statement).  I want to know when all await statements have
    /// completed.  Hence all SceneGods should call SceneLoaded(theirName) when their Start()
    /// methods have completed, after waiting for all async operations.
    /// </summary>
    public interface ISceneNavigator
    {
        void GoToScene(string sceneName, string loadingScreenHint = null);
        void SceneLoaded(string sceneName);
    }
}
