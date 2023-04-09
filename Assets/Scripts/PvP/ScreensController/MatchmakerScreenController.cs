using UnityEngine;
using UnityEngine.SceneManagement;


namespace BattleCruisers.UI.ScreensScene.Multiplay.ArenaScreen
{
    public class MatchmakerScreenController : MonoBehaviour
    {
        void Start()
        {
            SceneManager.LoadSceneAsync("MultiplayMatchmakingLoadingScene", LoadSceneMode.Additive);
        }
    }
}


