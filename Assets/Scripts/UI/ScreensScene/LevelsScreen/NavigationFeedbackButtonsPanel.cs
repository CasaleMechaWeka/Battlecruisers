using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.LevelsScreen
{
    public class NavigationFeedbackButtonsPanel : MonoBehaviour
    {
        public void Initialise(LevelsScreenController levelsScreenController, int numOfSets)
        {
            Assert.IsNotNull(levelsScreenController);

            NavigationFeedbackButtonController[] feedbackButtons = GetComponentsInChildren<NavigationFeedbackButtonController>();
            Assert.AreEqual(numOfSets, feedbackButtons.Length);

            for (int i = 0; i < numOfSets; ++i)
            {
                feedbackButtons[i].Initialise(levelsScreenController, i);
            }
        }
    }
}