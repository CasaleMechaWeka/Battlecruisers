using BattleCruisers.Data;
using BattleCruisers.Data.Helpers;
using BattleCruisers.Data.Models;
using BattleCruisers.Scenes;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils.Fetchers;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.BattleHubScreen
{
    public class ShopPanelScreenController : ScreenController
    {
        public CanvasGroupButton backButton;

        public void Initialise(
            IScreensSceneGod screensSceneGod,
            ISingleSoundPlayer soundPlayer,
            IPrefabFactory prefabFactory,
            IDataProvider dataProvider,
            INextLevelHelper nextLevelHelper)
        {
            base.Initialise(screensSceneGod);
            backButton.Initialise(soundPlayer, Home, this);
        }

        public void Home()
        {
            _screensSceneGod.GotoHubScreen();
        }
    }
}