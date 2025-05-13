using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.Utils;
using System;

namespace BattleCruisers.UI.BattleScene
{
    public class InformatorDismisser
    {
        private readonly IClickableEmitter _background;
        private readonly UIManager _uiManager;

        public InformatorDismisser(IClickableEmitter background, UIManager uiManager)
        {
            Helper.AssertIsNotNull(background, uiManager);

            _background = background;
            _uiManager = uiManager;

            _background.Clicked += _background_Clicked;
        }

        private void _background_Clicked(object sender, EventArgs e)
        {
            _uiManager.HideItemDetails();
        }
    }
}