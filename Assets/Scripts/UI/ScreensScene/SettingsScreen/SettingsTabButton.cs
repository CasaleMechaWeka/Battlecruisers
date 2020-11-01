using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using System;
using UnityEngine;

namespace BattleCruisers.UI.ScreensScene.SettingsScreen
{
    public class SettingsTabButton : CanvasGroupButton
    {
        private RectTransform _transform;
        private Vector2 _defaulteSizeDelta;

        private const float NOT_SELECTED_SIZE_MULTIPLIER = 0.7f;

        public bool IsSelected
        {
            set
            {
                if (value)
                {
                    MakeBig();
                }
                else
                {
                    MakeSmall();
                }
            }
        }

        public override void Initialise(
            ISingleSoundPlayer soundPlayer,
            Action clickAction = null,
            IDismissableEmitter parent = null)
        {
            base.Initialise(soundPlayer, clickAction, parent);

            _rectTransform = transform.Parse<RectTransform>();
            _defaulteSizeDelta = _rectTransform.sizeDelta;
        }

        private void MakeBig()
        {
            if (_rectTransform.sizeDelta == _defaulteSizeDelta)
            {
                _rectTransform.sizeDelta /= NOT_SELECTED_SIZE_MULTIPLIER;
            }
        }

        private void MakeSmall()
        {
            if (_rectTransform.sizeDelta.magnitude > _defaulteSizeDelta.magnitude)
            {
                _rectTransform.sizeDelta *= NOT_SELECTED_SIZE_MULTIPLIER;
            }
        }
    }
}