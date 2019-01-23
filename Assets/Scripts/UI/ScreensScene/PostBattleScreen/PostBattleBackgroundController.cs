using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.PostBattleScreen
{
    public class PostBattleBackgroundController : MonoBehaviour
    {
        public void Initalise(bool isVictory)
        {
            Image victoryBackground = transform.FindNamedComponent<Image>("Victory");
            victoryBackground.enabled = isVictory;

            Image defeatBackground = transform.FindNamedComponent<Image>("Defeat");
            defeatBackground.enabled = !isVictory;
        }
    }
}