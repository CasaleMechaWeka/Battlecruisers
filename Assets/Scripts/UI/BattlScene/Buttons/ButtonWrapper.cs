using BattleCruisers.UI.Filters;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.UI;

namespace BattleCruisers.UI.BattleScene.Buttons
{
    // FELIX  Rename:  TogglableButton?  Interface too?
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

        public void Initialise(IBroadcastingFilter shouldBeEnabledFilter, UnityAction clickHandler = null, bool disableButton = true)
        {
            _disableButton = disableButton;

            Button = GetComponent<Button>();
            Assert.IsNotNull(Button);

            if (clickHandler != null)
            {
                Button.onClick.AddListener(clickHandler);
            }

            // Sets overriden property, hence at end of Initialise() :P
            base.Initialise(shouldBeEnabledFilter);
        }
    }
}
