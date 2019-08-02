using BattleCruisers.UI.BattleScene.Buttons.Toggles;
using BattleCruisers.UI.Filters;
using BattleCruisers.Utils;
using System;
using UnityCommon.PlatformAbstractions;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.BattleScene.GameSpeed
{
    public class GameSpeedButton : CanvasGroupButton, IToggleButton
    {
        private ITime _time;
        private FilterToggler _isEnabledToggler;

        private const float DEFAULT_TIME_SCALE = 1;

        public float timeScale;

        public Image selectedFeedback;
        protected override MaskableGraphic Graphic => selectedFeedback;

        public event EventHandler Clicked;

        public bool IsSelected
        {
            set
            {
                selectedFeedback.gameObject.SetActive(value);

                _time.TimeScale = value ? timeScale : DEFAULT_TIME_SCALE;
            }
        }

        public void Initialise(IBroadcastingFilter shouldBeEnabledFilter, ITime time)
        {
            base.Initialise();

            Assert.IsTrue(timeScale >= 0);
            Helper.AssertIsNotNull(selectedFeedback, shouldBeEnabledFilter, time);

            IsSelected = false;
            _isEnabledToggler = new FilterToggler(this, shouldBeEnabledFilter);
            _time = time;
        }

        protected override void OnClicked()
        {
            Clicked?.Invoke(this, EventArgs.Empty);
        }
    }
}