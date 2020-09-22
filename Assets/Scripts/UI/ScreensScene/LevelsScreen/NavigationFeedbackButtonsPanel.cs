using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.LevelsScreen
{
    // FELIX  delete :)
    public class NavigationFeedbackButtonsPanel : MonoBehaviour
    {
        public void Initialise(LevelsScreenController levelsScreenController)
        {
            Assert.IsNotNull(levelsScreenController);

            NavigationFeedbackButton[] feedbackButtons = GetComponentsInChildren<NavigationFeedbackButton>();

            for (int i = 0; i < feedbackButtons.Length; ++i)
            {
                // FELIX
                feedbackButtons[i].Initialise(levelsScreenController, i, true);
            }
        }
    }
}