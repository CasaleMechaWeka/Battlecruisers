using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.LevelsScreen
{
    public class NavigationFeedbackButtonController : MonoBehaviour 
    {
        private LevelsScreenController _levelScreenController;
        private int _setIndex;
        private bool _isSelected;
        private RectTransform _transform;

        private const float NOT_SELECTED_SIZE_MULTIPLIER = 0.5f;

        public void Initialise(LevelsScreenController levelScreenController, int setIndex)
        {
            _levelScreenController = levelScreenController;
            _setIndex = setIndex;
            _isSelected = true;

            _transform = transform as RectTransform;
            Assert.IsNotNull(_transform);

            _levelScreenController.VisibleSetChanged += _levelScreenController_VisibleSetChanged;
        }
		
        // FELIX  Use states :P
		private void _levelScreenController_VisibleSetChanged(object sender, EventArgs e)
		{
            bool wasSelected = _isSelected;
            _isSelected = _levelScreenController.VisibleSetIndex == _setIndex;

            if (wasSelected && !_isSelected)
            {
                _transform.sizeDelta *= NOT_SELECTED_SIZE_MULTIPLIER;
            }
            else if (!wasSelected && _isSelected)
            {
                _transform.sizeDelta /= NOT_SELECTED_SIZE_MULTIPLIER;
            }
		}

        public void NavigateToSet()
        {
            _levelScreenController.ShowSet(_setIndex);
        }
    }
}
