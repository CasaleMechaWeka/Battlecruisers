using BattleCruisers.UI.BattleScene.Buttons.Toggles;
using BattleCruisers.UI.Filters;
using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace BattleCruisers.UI.BattleScene.GameSpeed
{
    public class GameSpeedButtonNEW : UIElement, IToggleButton, IPointerClickHandler
    {
        private FilterToggler _isEnabledToggler;

        private const float DEFAULT_TIME_SCALE = 1;

        public float timeScale;
        public GameObject selectedFeedback;

        public event EventHandler Clicked;

        public bool IsSelected
        {
            set
            {
                selectedFeedback.SetActive(value);
                Time.timeScale = value ? timeScale : DEFAULT_TIME_SCALE;
            }
        }

        public void Initialise(IBroadcastingFilter shouldBeEnabledFilter)
        {
            base.Initialise();

            Assert.IsTrue(timeScale >= 0);
            Assert.IsNotNull(selectedFeedback);
            Assert.IsNotNull(shouldBeEnabledFilter);

            IsSelected = false;
            _isEnabledToggler = new FilterToggler(this, shouldBeEnabledFilter);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (Clicked != null)
            {
                Clicked.Invoke(this, EventArgs.Empty);
            }
        }
    }
}