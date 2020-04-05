using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.Utils;
using System;

// FELIX  Test?
namespace BattleCruisers.UI.BattleScene
{
    public class InformatorDismisser
    {
        private readonly IClickableEmitter _background;
        private readonly IUIManager _uiManager;

        public InformatorDismisser(IClickableEmitter background, IUIManager uiManager)
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