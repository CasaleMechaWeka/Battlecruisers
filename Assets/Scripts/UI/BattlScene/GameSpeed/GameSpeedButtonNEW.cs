using BattleCruisers.UI.BattleScene.Buttons.Toggles;
using BattleCruisers.Utils.PlatformAbstractions;
using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace BattleCruisers.UI.BattleScene.GameSpeed
{
    // FELIX  Remove legacy class and rename this :)
    public class GameSpeedButtonNEW : UIElement, IToggleButton, IPointerClickHandler
    {
        private ITime _time;

        public float timeScale;
        public GameObject selectedFeedback;

        public event EventHandler Clicked;

        public bool IsSelected
        {
            set
            {
                selectedFeedback.SetActive(value);

                if (value)
                {
                    Time.timeScale = timeScale;
                }
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