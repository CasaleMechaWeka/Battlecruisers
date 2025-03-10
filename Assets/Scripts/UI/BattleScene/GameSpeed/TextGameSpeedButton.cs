using BattleCruisers.UI.Common;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.BattleScene.GameSpeed
{
    public class TextGameSpeedButton : ButtonController
    {
        private Text _chevron;
        protected override MaskableGraphic Graphic => _chevron;

        protected override void GetAssets()
        {
            _chevron = GetComponentInChildren<Text>();
            Assert.IsNotNull(_chevron);
        }
    }
}