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
        private IActivenessDecider _activenessDecider;
        private CanvasGroup _canvasGroup;

		public Button Button { get; private set; }

        // FELIX  Make private
        public bool IsEnabled
        {
            set
            {
                Button.enabled = value;
                _canvasGroup.alpha = value ? Constants.ENABLED_UI_ALPHA : Constants.DISABLED_UI_ALPHA;
            }
        }

        public void Initialise(UnityAction clickHandler, IActivenessDecider activenessDecider)
        {
            Helper.AssertIsNotNull(clickHandler, activenessDecider);

            _activenessDecider = activenessDecider;
            _activenessDecider.PotentialActivenessChange += _activenessDecider_PotentialActivenessChange;

            _canvasGroup = GetComponent<CanvasGroup>();
            Assert.IsNotNull(_canvasGroup);

            Button = GetComponent<Button>();
            Assert.IsNotNull(Button);
            Button.onClick.AddListener(clickHandler);
        }

        private void _activenessDecider_PotentialActivenessChange(object sender, EventArgs e)
        {
            IsEnabled = _activenessDecider.ShouldBeEnabled;
        }
    }
}
