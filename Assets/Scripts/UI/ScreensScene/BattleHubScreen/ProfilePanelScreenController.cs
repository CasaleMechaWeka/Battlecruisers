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

        public InfiniteCaptainScreenController captainsPanel;

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

        public void Initialise(
            IScreensSceneGod screensSceneGod,
            ISingleSoundPlayer soundPlayer,
            IPrefabFactory prefabFactory,
            IDataProvider dataProvider,
            INextLevelHelper nextLevelHelper)
        {
            base.Initialise(screensSceneGod);

            Helper.AssertIsNotNull(captainsButton);

            _soundPlayer = soundPlayer;

            captainsButton.Initialise(_soundPlayer, ChangeCaptain);

            // XP progress bar setup


/*            rank = ranker.CalculateRank(ApplicationModelProvider.ApplicationModel.DataProvider.GameModel.LifetimeDestructionScore);
            currentXP = ApplicationModelProvider.ApplicationModel.DataProvider.GameModel.XPToNextLevel;
            levelXP = (int)(ranker.CalculateLevelXP(rank));
            xpBar.setValues(currentXP, levelXP);
            currentXPString.text = FormatNumber(currentXP).ToString();
            levelXPString.text = FormatNumber(levelXP).ToString();*/
        }

        private void ChangeCaptain()
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