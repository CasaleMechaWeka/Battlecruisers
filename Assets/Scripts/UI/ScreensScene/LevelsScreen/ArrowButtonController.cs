using BattleCruisers.UI.Common;
using BattleCruisers.Utils;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.LevelsScreen
{
    public class ArrowButtonController : ButtonController
    {
        protected override void GetAssets()
        {
            _buttonImage = transform.FindNamedComponent<Image>("ArrowImage");
        }
    }
}