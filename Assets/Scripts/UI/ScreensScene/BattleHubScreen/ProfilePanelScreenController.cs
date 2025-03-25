using BattleCruisers.Data;
using BattleCruisers.Scenes;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using System;
using UnityEngine;
using UnityEngine.UI;
using BattleCruisers.Utils.Localisation;
using BattleCruisers.Data.Static;
using BattleCruisers.Utils.Fetchers.Sprites;

namespace BattleCruisers.UI.ScreensScene.BattleHubScreen
{
    public class ProfilePanelScreenController : ScreenController
    {

        public static ProfilePanelScreenController Instance { get; private set; }
        private ISingleSoundPlayer _soundPlayer;
        private PrefabFactory _prefabFactory;

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
        private XPBar xpBar;
        private int currentXP;
        private int levelXP;
        private int rank;
        [SerializeField]
        private Text currentXPString;
        [SerializeField]
        private Text levelXPString;
        [SerializeField]
        private Text totalDamage;
        private String million, billion, trillion, quadrillion;
        public GameObject currentCaptainImage;
        public Image badgeIcon, medalIcon;
        public Text rankTitle;
        public Image rankImage;
        public Text notorietyScore;
        private string playerID;

        public async void Initialise(
            IScreensSceneGod screensSceneGod,
            ISingleSoundPlayer soundPlayer,
            PrefabFactory prefabFactory)
        {
            base.Initialise(screensSceneGod);

            Helper.AssertIsNotNull(captainEditButton, playerNameEditButton, captainNamePopupPanel);
            Helper.AssertIsNotNull(screensSceneGod, soundPlayer, prefabFactory);
            _soundPlayer = soundPlayer;
            _prefabFactory = prefabFactory;

            million = LocTableCache.CommonTable.GetString("Million");
            billion = LocTableCache.CommonTable.GetString("Billion");
            trillion = LocTableCache.CommonTable.GetString("Trillion");
            quadrillion = LocTableCache.CommonTable.GetString("Quadrillion");

            captainNamePopupPanel.Initialise(screensSceneGod, soundPlayer, prefabFactory);
            captainEditButton.Initialise(_soundPlayer, OnClickCaptainEditBtn);
            playerNameEditButton.Initialise(_soundPlayer, OnClickNameEditBtn);
            selectButton.Initialise(_soundPlayer, OnClickSelectButton);
            captainsPanel.gameObject.SetActive(false);
            captainNamePopupPanel.gameObject.SetActive(false);

            spinnerOfSelect.SetActive(false);
            lableOfSelect.SetActive(true);

            playerName.text = DataProvider.GameModel.PlayerName;
            int rank = CalculateRank(DataProvider.GameModel.LifetimeDestructionScore);
            rankTitle.text = LocTableCache.CommonTable.GetString(StaticPrefabKeys.Ranks.AllRanks[rank].RankNameKeyBase);
            rankImage.sprite = await SpriteFetcher.GetSpriteAsync("Assets/Resources_moved/Sprites/UI/ScreensScene/DestructionScore/" + StaticPrefabKeys.Ranks.AllRanks[rank].RankImage + ".png");

            int nextLevelXP;
            int currentXP;
            long lDes = DataProvider.GameModel.LifetimeDestructionScore;
            if (lDes > 0)
            {
                nextLevelXP = (int)CalculateLevelXP(rank);
                currentXP = (int)CalculateXpToNextLevel((int)lDes);
            }
            else
            {
                nextLevelXP = (int)CalculateLevelXP(rank);
                currentXP = 0;
            }
            totalDamage.text = FormatNumber(lDes);
            xpBar.setValues(currentXP, nextLevelXP);
            currentXPString.text = FormatNumber(currentXP);
            levelXPString.text = FormatNumber(nextLevelXP);

            Text scoreString = notorietyScore?.GetComponent<Text>();
            scoreString.text = Mathf.Floor(DataProvider.GameModel.BattleWinScore).ToString();

        }

        private int CalculateRank(long score)
        {

            for (int i = 0; i <= StaticPrefabKeys.Ranks.AllRanks.Count - 1; i++)
            {
                long x = 2500 + 2500 * i * i;
                //Debug.Log(x);
                if (score < x)
                {
                    return i;
                }
            }
            return StaticPrefabKeys.Ranks.AllRanks.Count - 1;
        }

        // return what the x value will be in CalculateRank()
        // used for setting the max val of any XP progress bars
        public long CalculateLevelXP(int i)
        {
            long x = 2500 + 2500 * i * i;
            return x;
        }

        // returns the remainder of the score towards the next level,
        // based on the current lifetime score passed in
        public long CalculateXpToNextLevel(long score)
        {
            int currentRank = CalculateRank(score); // Calculate the current rank using the existing method

            if (currentRank >= StaticPrefabKeys.Ranks.AllRanks.Count - 1)
            {
                // If the current rank is already the highest, there is no remainder
                return 0;
            }

            long currentRankThreshold = 2500 + 2500 * currentRank * currentRank;
            long nextRankThreshold = 2500 + 2500 * (currentRank + 1) * (currentRank + 1);

            long scoreDifference = nextRankThreshold - currentRankThreshold;
            long scoreRemainder = currentRankThreshold - score;
            if (scoreRemainder < 0)
            {
                scoreRemainder = 0;
            }
            return scoreRemainder;
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
            captainsPanel.DisplayOwnedCaptains();
        }

        async void OnClickSelectButton()
        {
            spinnerOfSelect.SetActive(true);
            lableOfSelect.SetActive(false);
            if (await captainsPanel.SaveCurrentItem())
            {
                PlayerInfoPanelController.Instance.UpdateInfo(_prefabFactory);
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
            num = num * 1000;
            long i = (long)Math.Pow(10, (int)Math.Max(0, Math.Log10(num) - 2));
            num = num / i * i;
            if (num >= 1000000000000)
                return "$" + (num / 1000000000000D).ToString("0.##") + " " + quadrillion;
            if (num >= 1000000000)
                return "$" + (num / 1000000000D).ToString("0.##") + " " + trillion;
            if (num >= 1000000)
                return "$" + (num / 1000000D).ToString("0.##") + " " + billion;
            if (num >= 1000)
                return "$" + (num / 1000D).ToString("0.##") + " " + million;

            return "$" + num.ToString("#,0");
        }
    }
}