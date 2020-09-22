using System;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.LevelsScreen
{
    public class NavigationFeedbackButton : MonoBehaviour 
    {
        private LevelsScreenController _levelScreenController;
        private int _setIndex;
        private bool _isSelected;
        private RectTransform _transform;

        public Button button;

        private const float NOT_SELECTED_SIZE_MULTIPLIER = 0.5f;

        public void Initialise(LevelsScreenController levelScreenController, int setIndex, bool isEnabled)
        {
            Assert.IsNotNull(button);
            Assert.IsNotNull(levelScreenController);

            _levelScreenController = levelScreenController;
            _setIndex = setIndex;
            _isSelected = true;
            _transform = transform.Parse<RectTransform>();

            button.interactable = isEnabled;

            _levelScreenController.VisibleSetChanged += _levelScreenController_VisibleSetChanged;
        }
		
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
