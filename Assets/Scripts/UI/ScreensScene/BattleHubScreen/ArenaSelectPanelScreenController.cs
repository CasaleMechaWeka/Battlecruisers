using BattleCruisers.Data;
using BattleCruisers.Scenes;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Unity.Services.Authentication;
using UnityEngine.Assertions;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.Common;
using BattleCruisers.UI.Commands;
using BattleCruisers.Utils.Localisation;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene;
using BattleCruisers.UI.ScreensScene.CoinBattleScreen;
using System.Threading.Tasks;
using System.Collections.Generic;
using Unity.Services.Qos;
using BattleCruisers.Network.Multiplay.ApplicationLifecycle;

namespace BattleCruisers.UI.ScreensScene.BattleHubScreen
{
    public class ArenaSelectPanelScreenController : ScreenController
    {
        public Text costCoinsText;
        public Text costCreditsText;
        public Text prizeCoinsText;
        public Text prizeCreditsText;

        public RectTransform[] arenas;
        private int indexCurrentArena = 0;
        private Command _nextSetCommand, _previousSetCommand;
        public int IndexCurrentArena
        {
            get
            {
                return indexCurrentArena;
            }
            set
            {
                indexCurrentArena = value;
                _nextSetCommand.EmitCanExecuteChanged();
                _previousSetCommand.EmitCanExecuteChanged();
            }
        }
        public CanvasGroupButton battleButton;
        public ButtonController navRightButton;
        public ButtonController navLeftButton;
        public GameObject loadingSpinner;
        private ScreensSceneGod _screenSceneGod;
        private CoinBattleScreenController _coinBattleController;

        private bool isTransitioning = false;
        private bool isClickedBattleButton = false;

        public static Task<(bool success, IList<IQosResult> qosResults)> LatencyCheck;
        public static bool PrivateMatch = false;

        public void Initialise(
            ScreensSceneGod screensSceneGod,
            SingleSoundPlayer soundPlayer,
            CoinBattleScreenController coinBattleController = null)
        {
            loadingSpinner.SetActive(false);
            base.Initialise(screensSceneGod);
            Helper.AssertIsNotNull(battleButton, navRightButton, navLeftButton);

            _screenSceneGod = screensSceneGod;
            _coinBattleController = coinBattleController;

            battleButton.Initialise(soundPlayer, StartBattle);

            _nextSetCommand = new Command(NextSetCommandExecute, CanNextSetCommandExecute);
            navRightButton.Initialise(soundPlayer, _nextSetCommand);

            _previousSetCommand = new Command(PreviousSetCommandExecute, CanPreviousSetCommandExecute);
            navLeftButton.Initialise(soundPlayer, _previousSetCommand);

            DOTween.Init();
            isClickedBattleButton = false;
            isTransitioning = false;
        }

        public void OnEnable()
        {
            PrivateMatch = false;
            UpdateValueStrings(IndexCurrentArena);
            LatencyCheck = FetchLatencyByRegion();

            // Reset battle button state when panel becomes active (e.g., after FLEE from PVP)
            ResetBattleButtonState();
        }

        public void ResetBattleButtonState()
        {
            isClickedBattleButton = false;
            if (battleButton != null)
                battleButton.gameObject.SetActive(true);
            if (loadingSpinner != null)
                loadingSpinner.SetActive(false);
        }

        private void NextSetCommandExecute()
        {
            if (isTransitioning)
                return;
            isTransitioning = true;

            int cur_idx = IndexCurrentArena;
            IndexCurrentArena++;
            if (IndexCurrentArena > StaticData.NUM_OF_PvPLEVELS)
                IndexCurrentArena = StaticData.NUM_OF_PvPLEVELS;

            ShowArena(cur_idx, IndexCurrentArena);
        }

        private bool CanNextSetCommandExecute()
        {
            return
                IndexCurrentArena < StaticData.NUM_OF_PvPLEVELS - 1;
        }

        private void PreviousSetCommandExecute()
        {
            if (isTransitioning)
                return;
            isTransitioning = true;

            int cur_idx = IndexCurrentArena;
            IndexCurrentArena--;
            if (IndexCurrentArena < 0)
                IndexCurrentArena = 0;
            ShowArena(cur_idx, IndexCurrentArena);
        }

        private bool CanPreviousSetCommandExecute()
        {
            return IndexCurrentArena > 0;
        }

        private void ShowArena(int cur_idx, int next_idx)
        {
            Assert.IsTrue(next_idx >= 0 && next_idx < StaticData.NUM_OF_PvPLEVELS);
            if (cur_idx > IndexCurrentArena)
            {
                // nav left
                arenas[IndexCurrentArena].anchoredPosition = new Vector2(-5000f, 0f);
                arenas[IndexCurrentArena].SetAsLastSibling();
                arenas[IndexCurrentArena].gameObject.SetActive(true);
                arenas[IndexCurrentArena].DOAnchorPosX(0f, 0.3f)
                    .OnComplete(() =>
                    {
                        arenas[cur_idx].gameObject.SetActive(false);
                        UpdateValueStrings(next_idx);
                    });
            }
            else if (cur_idx < IndexCurrentArena)
            {
                // nav right
                arenas[IndexCurrentArena].anchoredPosition = new Vector2(5000f, 0f);
                arenas[IndexCurrentArena].SetAsLastSibling();
                arenas[IndexCurrentArena].gameObject.SetActive(true);
                arenas[IndexCurrentArena].DOAnchorPosX(0f, 0.3f)
                    .OnComplete(() =>
                    {
                        arenas[cur_idx].gameObject.SetActive(false);
                        UpdateValueStrings(next_idx);
                    });
            }

            //   DataProvider.GameModel.GameMap = IndexCurrentArena;
        }

        private void UpdateValueStrings(int index)
        {
            // index 0 is a Template with 0s in all fields.
            costCoinsText.text = StaticData.Arenas[index + 1].costcoins.ToString();
            costCreditsText.text = StaticData.Arenas[index + 1].costcredits.ToString();
            prizeCoinsText.text = StaticData.Arenas[index + 1].prizecoins.ToString();
            prizeCreditsText.text = StaticData.Arenas[index + 1].prizecredits.ToString();
            isTransitioning = false;
        }

        private void StartBattle()
        {
            if (!isClickedBattleButton)
            {
                isClickedBattleButton = true;
                loadingSpinner.SetActive(true);
                battleButton.gameObject.SetActive(false);

                // Handle 1vAI (CoinBattle) mode
                if (ApplicationModel.Mode == GameMode.CoinBattle)
                {
                    if (_coinBattleController != null)
                    {
                        _coinBattleController.Battle();
                    }
                    else
                    {
                        Debug.LogError("CoinBattleController not set for 1vAI battle");
                        loadingSpinner.SetActive(false);
                        battleButton.gameObject.SetActive(true);
                        isClickedBattleButton = false;
                    }
                    return;
                }

                if (AuthenticationService.Instance.IsSignedIn)
                {
                    if (DataProvider.GameModel.Coins >= StaticData.Arenas[indexCurrentArena + 1].costcoins
                        && DataProvider.GameModel.Credits >= StaticData.Arenas[indexCurrentArena + 1].costcredits)
                    {
                        DataProvider.GameModel.GameMap = IndexCurrentArena;
                        PvPBattleSceneGodTunnel.isCost = false;
                        PvPBattleCompletionHandler._isCompleted = false;
                        _screenSceneGod.LoadPvPBattleScene();
                    }
                    else
                    {
                        if (DataProvider.GameModel.Coins < StaticData.Arenas[indexCurrentArena + 1].costcoins)
                            ScreensSceneGod.Instance.messageBox.ShowMessage(LocTableCache.ScreensSceneTable.GetString("InsufficientCoins"));
                        if (DataProvider.GameModel.Credits < StaticData.Arenas[indexCurrentArena + 1].costcredits)
                            ScreensSceneGod.Instance.messageBox.ShowMessage(LocTableCache.ScreensSceneTable.GetString("InsufficientCredits"));
                        loadingSpinner.SetActive(false);
                        battleButton.gameObject.SetActive(true);
                        isClickedBattleButton = false;
                    }
                }
            }
        }

        private static async Task<(bool success, IList<IQosResult> qosResults)> FetchLatencyByRegion()
        {
            var qosResults = await QosService.Instance.GetSortedQosResultsAsync("relay", null);
            if (qosResults == null || qosResults.Count == 0)
                return (false, null);

            return (true, qosResults);
        }
    }
}
