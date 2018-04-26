using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.BattleScene.Buttons
{
    public class ButtonWrapper : MonoBehaviour, IButtonWrapper
    {
        private CanvasGroup _canvasGroup;

		public Button Button { get; private set; }

        public bool IsEnabled
        {
            set
            {
                Button.enabled = value;
                _canvasGroup.alpha = value ? Constants.ENABLED_UI_ALPHA : Constants.DISABLED_UI_ALPHA;
            }
        }

        public void Initialise()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            Assert.IsNotNull(_canvasGroup);

            Button = GetComponent<Button>();
            Assert.IsNotNull(Button);
        }
    }
}
