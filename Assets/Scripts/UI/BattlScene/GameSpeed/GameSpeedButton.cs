using BattleCruisers.UI.BattleScene.Buttons.Toggles;
using BattleCruisers.UI.Filters;
using BattleCruisers.Utils;
using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.BattleScene.GameSpeed
{
    public class GameSpeedButton : CanvasGroupButton, IToggleButton
    {
        private FilterToggler _isEnabledToggler;

        private const float DEFAULT_TIME_SCALE = 1;

        public float timeScale;
        public Image selectedFeedback;

        public event EventHandler Clicked;

        protected override MaskableGraphic Graphic => selectedFeedback;

        public bool IsSelected
        {
            set
            {
                selectedFeedback.gameObject.SetActive(value);
                Time.timeScale = value ? timeScale : DEFAULT_TIME_SCALE;
            }
        }

        public void Initialise(IBroadcastingFilter shouldBeEnabledFilter)
        {
            base.Initialise();

            Assert.IsTrue(timeScale >= 0);
            Helper.AssertIsNotNull(selectedFeedback, shouldBeEnabledFilter);

            IsSelected = false;
            _isEnabledToggler = new FilterToggler(this, shouldBeEnabledFilter);
        }

        protected override void OnClicked()
        {
            Clicked?.Invoke(this, EventArgs.Empty);
        }
    }
}