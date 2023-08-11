using BattleCruisers.Data.Helpers;
using BattleCruisers.Data;
using BattleCruisers.Scenes;
using BattleCruisers.UI.ScreensScene.SettingsScreen;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils.Fetchers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using BattleCruisers.Utils;
using TMPro;

namespace BattleCruisers.UI.ScreensScene.BattleHubScreen
{
    public class InputNamePopupPanelController : MonoBehaviour
    {
        public Image charlieImage;
        public CanvasGroupButton applyBtn;
        public TMP_InputField inputField;

        public void Initialise(
        IScreensSceneGod screensSceneGod,
        ISingleSoundPlayer soundPlayer,
        IPrefabFactory prefabFactory,
        IDataProvider dataProvider,
        INextLevelHelper nextLevelHelper)
        {
            Helper.AssertIsNotNull(screensSceneGod, soundPlayer, prefabFactory, dataProvider, nextLevelHelper);
            applyBtn.Initialise(soundPlayer, ApplyName);
        }

        public void SetCharlie(Sprite sp)
        {
            Assert.IsNotNull(sp);
            charlieImage.sprite = sp;
        }

        void ApplyName()
        {
            
        }
    }
}
