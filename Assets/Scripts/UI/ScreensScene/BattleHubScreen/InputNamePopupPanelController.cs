using BattleCruisers.Data.Helpers;
using BattleCruisers.Data;
using BattleCruisers.Scenes;
using BattleCruisers.UI.ScreensScene.SettingsScreen;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils.Fetchers;
using UnityEngine;
using UnityEngine.UI;
using BattleCruisers.Utils;
using TMPro;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using Unity.Services.Authentication;
using System;
using BattleCruisers.Data.Models.PrefabKeys;
using UnityEngine.PlayerLoop;

namespace BattleCruisers.UI.ScreensScene.BattleHubScreen
{
    public class InputNamePopupPanelController : MonoBehaviour
    {
        public Image charlieImage;
        public CanvasGroupButton applyBtn;
        public TMP_InputField inputField;
        public GameObject spinner;
        public GameObject btnLabel;

        private IScreensSceneGod _screenSceneGod;
        private ISingleSoundPlayer _soundPlayer;
        private IPrefabFactory _prefabFactory;
        private IDataProvider _dataProvider;
        private CaptainExoKey loadedCaptain;

        public void Initialise(
        IScreensSceneGod screensSceneGod,
        ISingleSoundPlayer soundPlayer,
        IPrefabFactory prefabFactory,
        IDataProvider dataProvider,
        INextLevelHelper nextLevelHelper)
        {
            Helper.AssertIsNotNull(screensSceneGod, soundPlayer, prefabFactory, dataProvider, nextLevelHelper);
            _screenSceneGod = screensSceneGod;
            _soundPlayer = soundPlayer;
            _prefabFactory = prefabFactory;
            _dataProvider = dataProvider;

            applyBtn.Initialise(_soundPlayer, ApplyName);
            CaptainExo captain = _prefabFactory.GetCaptainExo(_dataProvider.GameModel.PlayerLoadout.CurrentCaptain);
            loadedCaptain = _dataProvider.GameModel.PlayerLoadout.CurrentCaptain;
            charlieImage.sprite = captain.CaptainExoImage;

            inputField.text = _dataProvider.GameModel.PlayerName;
            spinner.SetActive(false);
            btnLabel.SetActive(true);
        }

        public void Update()
        {
            if(loadedCaptain != _dataProvider.GameModel.PlayerLoadout.CurrentCaptain)
            {
                CaptainExo captain = _prefabFactory.GetCaptainExo(_dataProvider.GameModel.PlayerLoadout.CurrentCaptain);
                loadedCaptain = _dataProvider.GameModel.PlayerLoadout.CurrentCaptain;
                charlieImage.sprite = captain.CaptainExoImage;
            }
        }

        async void ApplyName()
        {
            btnLabel.SetActive(false);
            spinner.SetActive(true);
            applyBtn.enabled = false;
            string oldPlayerName = string.Empty;
            CaptainExo captain = _prefabFactory.GetCaptainExo(_dataProvider.GameModel.PlayerLoadout.CurrentCaptain);
            if (inputField.text != _dataProvider.GameModel.PlayerName)
            {
                if (await LandingSceneGod.CheckForInternetConnection() && AuthenticationService.Instance.IsSignedIn)
                {
                    try
                    {
                        oldPlayerName = _dataProvider.GameModel.PlayerName;
                        _dataProvider.GameModel.PlayerName = inputField.text;
                        _dataProvider.SaveGame();
                        await _dataProvider.CloudSave();
                        string name = inputField.text + "#" + captain.captainName;
                        Debug.Log(name);    
                        await AuthenticationService.Instance.UpdatePlayerNameAsync(name);
                        PlayerInfoPanelController.Instance?.UpdateInfo(_dataProvider, _prefabFactory);
                        ProfilePanelScreenController.Instance.playerName.text = _dataProvider.GameModel.PlayerName;
                    }
                    catch (Exception ex)
                    {
                        _dataProvider.GameModel.PlayerName = oldPlayerName;
                        _dataProvider.SaveGame();
                        PlayerInfoPanelController.Instance?.UpdateInfo(_dataProvider, _prefabFactory);
                        ProfilePanelScreenController.Instance.playerName.text = _dataProvider.GameModel.PlayerName;
                        Debug.LogException(ex);
                    }
                }
        
            }

            gameObject.SetActive(false);
            btnLabel.SetActive(true);
            spinner.SetActive(false);
            applyBtn.enabled = true;
        }
    }
}
