using System;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.UI;

namespace BattleCruisers.UI.BattleScene.Buttons
{
    public class ButtonWrapper : MonoBehaviour, IButtonWrapper
    {
        private IFilter _activenessDecider;
        private CanvasGroup _canvasGroup;

		public Button Button { get; private set; }

        private bool IsEnabled
        {
            set
            {
                Button.enabled = value;
                _canvasGroup.alpha = value ? Constants.ENABLED_UI_ALPHA : Constants.DISABLED_UI_ALPHA;
            }
        }

        public void Initialise(UnityAction clickHandler, IFilter activenessDecider)
        {
            Helper.AssertIsNotNull(clickHandler, activenessDecider);

            _activenessDecider = activenessDecider;
            _activenessDecider.PotentialMatchChange += _activenessDecider_PotentialActivenessChange;

            _canvasGroup = GetComponent<CanvasGroup>();
            Assert.IsNotNull(_canvasGroup);

            Button = GetComponent<Button>();
            Assert.IsNotNull(Button);
            Button.onClick.AddListener(clickHandler);

            UpdateActiveness();
        }

        private void _activenessDecider_PotentialActivenessChange(object sender, EventArgs e)
        {
            UpdateActiveness();
        }

        private void UpdateActiveness()
        {
            IsEnabled = _activenessDecider.IsMatch;
        }
    }
}
