using BattleCruisers.Data;
using BattleCruisers.Scenes;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using System;
using UnityEngine;
using UnityEngine.UI;
using BattleCruisers.Utils.Localisation;
using BattleCruisers.Data.Static;
using BattleCruisers.Utils.Fetchers.Sprites;
using BattleCruisers.PostBattleScreen;

namespace BattleCruisers.UI.ScreensScene.BattleHubScreen
{
    public class ProfilePanelScreenController : ScreenController
    {

        public static ProfilePanelScreenController Instance { get; private set; }
        private SingleSoundPlayer _soundPlayer;

        public CanvasGroupButton captainEditButton;
        public CanvasGroupButton playerNameEditButton;
        public CanvasGroupButton selectButton;
        public CaptainSelectorPanel captainsPanel;
        public InputNamePopupPanelController captainNamePopupPanel;

        public GameObject spinnerOfSelect;
        public GameObject lableOfSelect;

        public Text playerName;
        // xp and rank vars
        [SerializeField]
        private Slider xpBar;
        private int currentXP;
        private int levelXP;
        private int rank;
        [SerializeField]
        private Text currentXPString;
        [SerializeField]
        private Text levelXPString;
        [SerializeField]
        private Text totalDamage;
        private string million, billion, trillion, quadrillion;
        public GameObject currentCaptainImage;
        public Image badgeIcon, medalIcon;
        public Text rankTitle;
        public Image rankImage;
        public Text notorietyScore;
        public Text Bounty;
        private string playerID;

        public async void Initialise(
            ScreensSceneGod screensSceneGod,
            SingleSoundPlayer soundPlayer)
        {
            base.Initialise(screensSceneGod);

            Helper.AssertIsNotNull(captainEditButton, playerNameEditButton, captainNamePopupPanel, Bounty);
            Helper.AssertIsNotNull(screensSceneGod, soundPlayer);
            _soundPlayer = soundPlayer;

            million = LocTableCache.CommonTable.GetString("Million");
            billion = LocTableCache.CommonTable.GetString("Billion");
            trillion = LocTableCache.CommonTable.GetString("Trillion");
            quadrillion = LocTableCache.CommonTable.GetString("Quadrillion");

            captainNamePopupPanel.Initialise(soundPlayer);
            captainEditButton.Initialise(_soundPlayer, OnClickCaptainEditBtn);
            playerNameEditButton.Initialise(_soundPlayer, OnClickNameEditBtn);
            selectButton.Initialise(_soundPlayer, OnClickSelectButton);
            captainsPanel.gameObject.SetActive(false);
            captainNamePopupPanel.gameObject.SetActive(false);

            spinnerOfSelect.SetActive(false);
            lableOfSelect.SetActive(true);

            playerName.text = DataProvider.GameModel.PlayerName;
            int rank = DestructionRanker.CalculateRank(DataProvider.GameModel.LifetimeDestructionScore);
            rankTitle.text = LocTableCache.CommonTable.GetString(StaticPrefabKeys.Ranks.AllRanks[rank].RankNameKeyBase);
            rankImage.sprite = await SpriteFetcher.GetSpriteAsync("Assets/Resources_moved/Sprites/UI/ScreensScene/DestructionScore/" + StaticPrefabKeys.Ranks.AllRanks[rank].RankImage + ".png");

            int nextLevelXP;
            int currentXP;
            long lDes = DataProvider.GameModel.LifetimeDestructionScore;
            if (lDes > 0)
            {
                nextLevelXP = (int)DestructionRanker.CalculateLevelXP(rank);
                currentXP = (int)DestructionRanker.CalculateXpToNextLevel((int)lDes);
            }
            else
            {
                nextLevelXP = (int)DestructionRanker.CalculateLevelXP(rank);
                currentXP = 0;
            }
            totalDamage.text = FormatNumber(lDes);
            xpBar.maxValue = nextLevelXP;
            xpBar.value = currentXP;
            currentXPString.text = FormatNumber(currentXP);
            levelXPString.text = FormatNumber(nextLevelXP);

            notorietyScore.text = DataProvider.GameModel.BattleWinScore.ToString("F0");
            Bounty.text = DataProvider.GameModel.Bounty.ToString("F0");
        }

        private void Awake()
        {
            Instance = this;
        }
        void OnClickNameEditBtn()
        {
            captainNamePopupPanel.gameObject.SetActive(true);
        }

        void OnClickCaptainEditBtn()
        {
            captainsPanel.gameObject.SetActive(true);
            captainsPanel.DisplayExoList();
        }

        async void OnClickSelectButton()
        {
            spinnerOfSelect.SetActive(true);
            lableOfSelect.SetActive(false);
            if (await captainsPanel.SaveCurrentItem())
            {
                PlayerInfoPanelController.Instance.UpdateInfo();
            }
            captainsPanel.gameObject.SetActive(false);
            spinnerOfSelect.SetActive(false);
            lableOfSelect.SetActive(true);
        }
        public override void OnDismissing()
        {
            base.OnDismissing();
            captainsPanel.RemoveAllCaptainsFromRenderCamera();
            captainsPanel.gameObject.SetActive(false);
        }

        public override void OnPresenting(object activationParameter)
        {
            base.OnPresenting(activationParameter);
            captainsPanel.ShowCurrentCaptain();
        }


        private void ChangeCaptainSelection()
        {
            if (!captainsPanel.isActiveAndEnabled)
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
            if (num == 0)
            {
                return "$0";
            }

            bool isNegative = num < 0;
            double adjustedNum = Math.Abs((double)num) * 1000d;

            double rawLog = Math.Log10(adjustedNum);
            int exponent = (int)Math.Max(0d, rawLog - 2d);
            double divisor = Math.Pow(10d, exponent);
            if (divisor <= 0d || double.IsNaN(divisor) || double.IsInfinity(divisor))
            {
                divisor = 1d;
            }

            adjustedNum = Math.Floor(adjustedNum / divisor) * divisor;

            string prefix = isNegative ? "-$" : "$";

            if (adjustedNum >= 1000000000000d)
                return prefix + (adjustedNum / 1000000000000d).ToString("0.##") + " " + quadrillion;
            if (adjustedNum >= 1000000000d)
                return prefix + (adjustedNum / 1000000000d).ToString("0.##") + " " + trillion;
            if (adjustedNum >= 1000000d)
                return prefix + (adjustedNum / 1000000d).ToString("0.##") + " " + billion;
            if (adjustedNum >= 1000d)
                return prefix + (adjustedNum / 1000d).ToString("0.##") + " " + million;

            return prefix + adjustedNum.ToString("#,0");
        }
    }
}