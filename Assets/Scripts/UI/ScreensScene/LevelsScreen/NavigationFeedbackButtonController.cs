using System;
using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.UI.ScreensScene.LevelsScreen
{
    // FELIX  Rename, remove Controller suffix
    public class NavigationFeedbackButtonController : MonoBehaviour 
    {
        private NEWLevelsScreenController _levelScreenController;
        private int _setIndex;
        private bool _isSelected;
        private RectTransform _transform;

        private const float NOT_SELECTED_SIZE_MULTIPLIER = 0.5f;

        public void Initialise(NEWLevelsScreenController levelScreenController, int setIndex)
        {
            _levelScreenController = levelScreenController;
            _setIndex = setIndex;
            _isSelected = true;
            _transform = transform.Parse<RectTransform>();

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
