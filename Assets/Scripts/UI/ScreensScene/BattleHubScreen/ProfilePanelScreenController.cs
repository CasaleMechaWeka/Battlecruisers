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

            Helper.AssertIsNotNull(captainEditButton, playerNameEditButton, captainNamePopupPanel);
            Helper.AssertIsNotNull(screensSceneGod, soundPlayer, prefabFactory, dataProvider, nextLevelHelper);
            _soundPlayer = soundPlayer;
            _dataProvider = dataProvider;
            _prefabFactory = prefabFactory;

            captainNamePopupPanel.Initialise(screensSceneGod, soundPlayer, prefabFactory, dataProvider, nextLevelHelper);
            captainEditButton.Initialise(_soundPlayer, OnClickCaptainEditBtn);
            playerNameEditButton.Initialise(_soundPlayer, OnClickNameEditBtn);
            selectButton.Initialise(_soundPlayer, OnClickSelectButton);
            captainsPanel.gameObject.SetActive(false);
            captainNamePopupPanel.gameObject.SetActive(false);

            spinnerOfSelect.SetActive(false);
            lableOfSelect.SetActive(true);

            playerName.text = _dataProvider.GameModel.PlayerName;
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