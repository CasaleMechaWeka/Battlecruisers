using BattleCruisers.Data;
using BattleCruisers.Data.Helpers;
using BattleCruisers.Data.Models;
using BattleCruisers.Scenes;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.PostBattleScreen;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.BattleHubScreen
{
    public class ProfilePanelScreenController : ScreenController
    {
        private ISingleSoundPlayer _soundPlayer;

        public CanvasGroupButton captainsButton;
        public CanvasGroupButton nameButton;

        public CaptainSelectorPanel captainsPanel;        

        // xp and rank vars
        [SerializeField]
        private XPBar xpBar;
        private int currentXP;
        private int levelXP;
        private int rank;
        public DestructionRanker ranker;
        [SerializeField]
        private Text currentXPString;
        [SerializeField]
        private Text levelXPString;
        public Text million, billion, trillion, quadrillion;
        public GameObject currentCaptainImage;
        public Image badgeIcon, medalIcon;
        public Text rankTitle;

        public void Initialise(
            IScreensSceneGod screensSceneGod,
            ISingleSoundPlayer soundPlayer,
            IPrefabFactory prefabFactory,
            IDataProvider dataProvider,
            INextLevelHelper nextLevelHelper)
        {
            base.Initialise(screensSceneGod);

            Helper.AssertIsNotNull(captainsButton, nameButton);
            Helper.AssertIsNotNull(screensSceneGod, soundPlayer, prefabFactory, dataProvider, nextLevelHelper);
            _soundPlayer = soundPlayer;

            captainsButton.Initialise(_soundPlayer, OnClickCaptainBtn);
            nameButton.Initialise(_soundPlayer, OnClickNameBtn);
        }

        void OnClickNameBtn()
        {
            
        }

        void OnClickCaptainBtn() 
        { 

        }

        private void ChangeCaptainSelection()
        {
            if(!captainsPanel.isActiveAndEnabled)
            {
                captainsPanel.gameObject.SetActive(true);
            }
            else
            {
                captainsPanel.gameObject.SetActive(false);
            }
        }

        //taken from https://stackoverflow.com/questions/30180672/string-format-numbers-to-millions-thousands-with-rounding
        private string FormatNumber(long num)
        {
            num = num * 1000;
            long i = (long)Math.Pow(10, (int)Math.Max(0, Math.Log10(num) - 2));
            num = num / i * i;
            if (num >= 1000000000000)
                return "$" + (num / 1000000000000D).ToString("0.##") + " " + quadrillion.text;
            if (num >= 1000000000)
                return "$" + (num / 1000000000D).ToString("0.##") + " " + trillion.text;
            if (num >= 1000000)
                return "$" + (num / 1000000D).ToString("0.##") + " " + billion.text;
            if (num >= 1000)
                return "$" + (num / 1000D).ToString("0.##") + " " + million.text;

            return "$" + num.ToString("#,0");
        }
    }
}