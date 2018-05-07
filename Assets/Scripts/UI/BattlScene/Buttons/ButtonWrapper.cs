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
        private IFilter _shouldBeEnabledFilter;
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

        public void Initialise(UnityAction clickHandler, IFilter shouldBeEnabledFilter)
        {
            Helper.AssertIsNotNull(clickHandler, shouldBeEnabledFilter);

            _shouldBeEnabledFilter = shouldBeEnabledFilter;
            _shouldBeEnabledFilter.PotentialMatchChange += _shouldBeEnabledFilter_PotentialMatchChange;

            _canvasGroup = GetComponent<CanvasGroup>();
            Assert.IsNotNull(_canvasGroup);

            Button = GetComponent<Button>();
            Assert.IsNotNull(Button);
            Button.onClick.AddListener(clickHandler);

            UpdateIsEnabled();
        }

        private void _shouldBeEnabledFilter_PotentialMatchChange(object sender, EventArgs e)
        {
            UpdateIsEnabled();
        }

        private void UpdateIsEnabled()
        {
            IsEnabled = _shouldBeEnabledFilter.IsMatch;
        }
    }
}
