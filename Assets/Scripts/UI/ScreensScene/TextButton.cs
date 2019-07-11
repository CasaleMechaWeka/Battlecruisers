using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene
{
    public abstract class TextButton : ClickableTogglable
    {
        private MaskableGraphic _text;
        protected override MaskableGraphic Graphic => _text;

        public override void Initialise()
        {
            base.Initialise();

            _text = GetComponentInChildren<Text>();
            Assert.IsNotNull(_text);
        }
    }
}