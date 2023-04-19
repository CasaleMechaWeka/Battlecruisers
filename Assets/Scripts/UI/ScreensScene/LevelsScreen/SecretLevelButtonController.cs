using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace BattleCruisers.UI.ScreensScene.LevelsScreen
{
    public class SecretLevelButtonController : MonoBehaviour
    {
        public Button secretLevelButton;
        public int secretLevelNumber;

        private void Start()
        {
            secretLevelButton.onClick.AddListener(OnSecretLevelButtonClick);
        }

        private void OnSecretLevelButtonClick()
        {
            // Load the secret level scene using the secret level number
            SceneManager.LoadScene("Level_" + secretLevelNumber);
        }
    }
}