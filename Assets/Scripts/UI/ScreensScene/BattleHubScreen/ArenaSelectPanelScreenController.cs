using BattleCruisers.Data;
using BattleCruisers.Data.Helpers;
using BattleCruisers.Data.Models;
using BattleCruisers.Scenes;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Unity.Services.Core;
using Unity.Services.Authentication;
using UnityEngine.Assertions;
using System;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.Common;
using BattleCruisers.UI.Commands;
using BattleCruisers.Network.Multiplay.Matchplay.Shared;
using BattleCruisers.Utils.Localisation;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene;

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
        private ICommand _nextSetCommand, _previousSetCommand;
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
        private IScreensSceneGod _screenSceneGod;
        private ISingleSoundPlayer _singleSoundPlayer;
        private IPrefabFactory _prefabFactory;
        private IDataProvider _dataProvider;
        private INextLevelHelper _nextLevelHelper;

        public ILocTable screensSceneTable;

        private bool isTransitioning = false;
        private bool isClickedBattleButton = false;
        public void Initialise(
            IScreensSceneGod screensSceneGod,
            ISingleSoundPlayer soundPlayer,
            IPrefabFactory prefabFactory,
            IDataProvider dataProvider,
            INextLevelHelper nextLevelHelper)
        {
            loadingSpinner.SetActive(false);
            base.Initialise(screensSceneGod);
            Helper.AssertIsNotNull(battleButton, navRightButton, navLeftButton);

            _screenSceneGod = screensSceneGod;
            _singleSoundPlayer = soundPlayer;
            _prefabFactory = prefabFactory;
            _dataProvider = dataProvider;

            battleButton.Initialise(soundPlayer, StartBattle);

            _nextSetCommand = new Command(NextSetCommandExecute, CanNextSetCommandExecute);
            navRightButton.Initialise(soundPlayer, _nextSetCommand);

            _previousSetCommand = new Command(PreviousSetCommandExecute, CanPreviousSetCommandExecute);
            navLeftButton.Initialise(soundPlayer, _previousSetCommand);

            DOTween.Init();
            screensSceneTable = LandingSceneGod.Instance.screenSceneStrings;
            isClickedBattleButton = false;
            isTransitioning = false;
        }

        public void OnEnable()
        {
            UpdateValueStrings(IndexCurrentArena);
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

            //   _dataProvider.GameModel.GameMap = IndexCurrentArena;
        }

        private void UpdateValueStrings(int index)
        {
            // index 0 is a Template with 0s in all fields.
            costCoinsText.text = _dataProvider.GameModel.Arenas[index + 1].costcoins.ToString();
            costCreditsText.text = _dataProvider.GameModel.Arenas[index + 1].costcredits.ToString();
            prizeCoinsText.text = _dataProvider.GameModel.Arenas[index + 1].prizecoins.ToString();
            prizeCreditsText.text = _dataProvider.GameModel.Arenas[index + 1].prizecredits.ToString();
            isTransitioning = false;
        }

        private void StartBattle()
        {
            if(!isClickedBattleButton)
            {
                isClickedBattleButton = true;
                loadingSpinner.SetActive(true);
                battleButton.gameObject.SetActive(false);
                if (AuthenticationService.Instance.IsSignedIn)
                {
                    if (_dataProvider.GameModel.Coins >= _dataProvider.GameModel.Arenas[indexCurrentArena + 1].costcoins
                        && _dataProvider.GameModel.Credits >= _dataProvider.GameModel.Arenas[indexCurrentArena + 1].costcredits)
                    {
/*                        loadingSpinner.SetActive(true);
                        battleButton.gameObject.SetActive(false);*/
                        _dataProvider.GameModel.GameMap = IndexCurrentArena;
                        PvPBattleSceneGodTunnel.isCost = false;
                        PvPBattleCompletionHandler._isCompleted = false;
                        _screenSceneGod.LoadPvPBattleScene();
                    }
                    else
                    {
                        if (_dataProvider.GameModel.Coins < _dataProvider.GameModel.Arenas[indexCurrentArena + 1].costcoins)
                            MessageBox.Instance.ShowMessage(screensSceneTable.GetString("InsufficientCoins"));
                        if (_dataProvider.GameModel.Credits < _dataProvider.GameModel.Arenas[indexCurrentArena + 1].costcredits)
                            MessageBox.Instance.ShowMessage(screensSceneTable.GetString("InsufficientCredits"));
                        loadingSpinner.SetActive(false);
                        battleButton.gameObject.SetActive(true);
                        isClickedBattleButton = false;
                    }
                }
            }
        }
    }
}
