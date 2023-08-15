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
using BattleCruisers.Utils.Localisation;
using BattleCruisers.Data.Static;
using BattleCruisers.Utils.Fetchers.Sprites;

namespace BattleCruisers.UI.ScreensScene.BattleHubScreen
{
    public class ProfilePanelScreenController : ScreenController
    {

        public static ProfilePanelScreenController Instance { get; private set; }
        private ISingleSoundPlayer _soundPlayer;
        private IDataProvider _dataProvider;
        private IPrefabFactory _prefabFactory;

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
        public Text million, billion, trillion, quadrillion;
        public GameObject currentCaptainImage;
        public Image badgeIcon, medalIcon;
        public Text rankTitle;
        public Image rankImage;

        private ILocTable commonStrings;

        public async void Initialise(
            IScreensSceneGod screensSceneGod,
            ISingleSoundPlayer soundPlayer,
            IPrefabFactory prefabFactory,
            IDataProvider dataProvider,
            INextLevelHelper nextLevelHelper)
        {
            base.Initialise(screensSceneGod);

            Helper.AssertIsNotNull(captainEditButton, playerNameEditButton, captainNamePopupPanel);
            Helper.AssertIsNotNull(screensSceneGod, soundPlayer, prefabFactory, dataProvider, nextLevelHelper);
            _soundPlayer = soundPlayer;
            _dataProvider = dataProvider;
            _prefabFactory = prefabFactory;

            commonStrings = await LocTableFactory.Instance.LoadCommonTableAsync();

            captainNamePopupPanel.Initialise(screensSceneGod, soundPlayer, prefabFactory, dataProvider, nextLevelHelper);
            captainEditButton.Initialise(_soundPlayer, OnClickCaptainEditBtn);
            playerNameEditButton.Initialise(_soundPlayer, OnClickNameEditBtn);
            selectButton.Initialise(_soundPlayer, OnClickSelectButton);
            captainsPanel.gameObject.SetActive(false);
            captainNamePopupPanel.gameObject.SetActive(false);

            spinnerOfSelect.SetActive(false);
            lableOfSelect.SetActive(true);

            playerName.text = _dataProvider.GameModel.PlayerName;
            int rank = CalculateRank(_dataProvider.GameModel.LifetimeDestructionScore);
            rankTitle.text = commonStrings.GetString(StaticPrefabKeys.Ranks.AllRanks[rank].RankNameKeyBase);
            SpriteFetcher fetcher = new SpriteFetcher();
            rankImage.sprite = (await fetcher.GetSpriteAsync("Assets/Resources_moved/Sprites/UI/ScreensScene/DestructionScore/" + StaticPrefabKeys.Ranks.AllRanks[rank].RankImage + ".png")).Sprite;
        }

        private int CalculateRank(long score)
        {

            for (int i = 0; i <= StaticPrefabKeys.Ranks.AllRanks.Count; i++)
            {
                long x = 2500 + 2500 * i * i;
                //Debug.Log(x);
                if (score < x)
                {
                    return i;
                }
            }
            return StaticPrefabKeys.Ranks.AllRanks.Count;
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
                PlayerInfoPanelController.Instance.UpdateInfo(_dataProvider, _prefabFactory);
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