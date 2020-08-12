using BattleCruisers.UI.Sound;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene
{
    public abstract class TextButton : ElementWithClickSound
    {
        private MaskableGraphic _text;
        protected override MaskableGraphic Graphic => _text;

        public void Initialise(ISingleSoundPlayer soundPlayer, IDismissableEmitter parent = null)
        {
            base.Initialise(soundPlayer, parent);

            _text = GetComponentInChildren<Text>();
            Assert.IsNotNull(_text);
        }
    }
}