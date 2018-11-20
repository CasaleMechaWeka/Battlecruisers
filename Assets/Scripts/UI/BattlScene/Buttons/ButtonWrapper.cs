using BattleCruisers.UI.Filters;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.UI;

namespace BattleCruisers.UI.BattleScene.Buttons
{
    public class ButtonWrapper : TogglableElement, IButtonWrapper
    {
        private bool _disableButton;

        // NEWUI  Make private, should no longer be used :)
		public Button Button { get; private set; }

        protected override bool IsEnabled
        {
            set
            {
                base.IsEnabled = value;

                if (_disableButton)
                {
                    Button.enabled = value;
                }
            }
        }

        public void Initialise(UnityAction clickHandler, IBroadcastingFilter shouldBeEnabledFilter, bool disableButton = true)
        {
            base.Initialise(shouldBeEnabledFilter);

            Assert.IsNotNull(clickHandler);

            _disableButton = disableButton;

            Button = GetComponent<Button>();
            Assert.IsNotNull(Button);
            Button.onClick.AddListener(clickHandler);
        }
    }
}
