using BattleCruisers.UI.BattleScene.Buttons.Toggles;
using BattleCruisers.Utils.PlatformAbstractions;
using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace BattleCruisers.UI.BattleScene.GameSpeed
{
    public class GameSpeedButtonNEW : UIElement, IToggleButton, IPointerClickHandler
    {
        private ITime _time;

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

        public override void Initialise()
        {
            Assert.IsTrue(timeScale >= 0);
            Assert.IsNotNull(selectedFeedback);

            IsSelected = false;
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