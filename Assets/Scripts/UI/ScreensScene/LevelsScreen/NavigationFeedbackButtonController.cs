using UnityEngine;

namespace BattleCruisers.UI.ScreensScene.LevelsScreen
{
    public class NavigationFeedbackButtonController : MonoBehaviour 
    {
        private LevelsScreenController _levelScreenController;
        private int _setIndex;

        public void Initialise(LevelsScreenController levelScreenController, int setIndex)
        {
            _levelScreenController = levelScreenController;
            _setIndex = setIndex;
        }

        public void NavigateToSet()
        {
            _levelScreenController.ShowSet(_setIndex);
        }
	}
}
