using BattleCruisers.UI.Sound;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene
{
    public abstract class TextButton : ButtonWithClickSound
    {
        private MaskableGraphic _text;
        protected override MaskableGraphic Graphic => _text;

        public override void Initialise(ISoundPlayer soundPlayer)
        {
            base.Initialise(soundPlayer);

            _text = GetComponentInChildren<Text>();
            Assert.IsNotNull(_text);
        }
    }
}