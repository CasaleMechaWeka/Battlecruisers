using BattleCruisers.Data;
using BattleCruisers.Scenes;
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
using BattleCruisers.Utils.Localisation;

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
        private SingleSoundPlayer _soundPlayer;
        private CaptainExoKey loadedCaptain;

        public void Initialise(
        IScreensSceneGod screensSceneGod,
        SingleSoundPlayer soundPlayer)
        {
            Helper.AssertIsNotNull(screensSceneGod, soundPlayer);
            _screenSceneGod = screensSceneGod;
            _soundPlayer = soundPlayer;

            applyBtn.Initialise(_soundPlayer, ApplyName);
            CaptainExo captain = PrefabFactory.GetCaptainExo(DataProvider.GameModel.PlayerLoadout.CurrentCaptain);
            loadedCaptain = DataProvider.GameModel.PlayerLoadout.CurrentCaptain;
            charlieImage.sprite = captain.CaptainExoImage;

            inputField.text = DataProvider.GameModel.PlayerName;
            spinner.SetActive(false);
            btnLabel.SetActive(true);
            btnLabel.GetComponent<Text>().text = LocTableCache.CommonTable.GetString("UI/Buttons/SaveButton");
        }

        public void Update()
        {
            if (loadedCaptain != DataProvider.GameModel.PlayerLoadout.CurrentCaptain)
            {
                CaptainExo captain = PrefabFactory.GetCaptainExo(DataProvider.GameModel.PlayerLoadout.CurrentCaptain);
                loadedCaptain = DataProvider.GameModel.PlayerLoadout.CurrentCaptain;
                charlieImage.sprite = captain.CaptainExoImage;
            }
        }

        async void ApplyName()
        {
            btnLabel.SetActive(false);
            spinner.SetActive(true);
            applyBtn.enabled = false;
            string oldPlayerName = string.Empty;
            CaptainExo captain = PrefabFactory.GetCaptainExo(DataProvider.GameModel.PlayerLoadout.CurrentCaptain);
            if (inputField.text != DataProvider.GameModel.PlayerName)
            {
                try
                {
                    oldPlayerName = DataProvider.GameModel.PlayerName;
                    string temp = inputField.text;
                    temp.Replace("#", "");
                    DataProvider.GameModel.PlayerName = temp;
                    DataProvider.SaveGame();
                    string name = temp + "#" + captain.captainName;
                    Debug.Log(name);

                    // online functions
                    if (await LandingSceneGod.CheckForInternetConnection() && AuthenticationService.Instance.IsSignedIn)
                    {
                        await DataProvider.CloudSave();
                        await AuthenticationService.Instance.UpdatePlayerNameAsync(name);
                    }

                    PlayerInfoPanelController.Instance?.UpdateInfo();
                    ProfilePanelScreenController.Instance.playerName.text = DataProvider.GameModel.PlayerName;
                }
                catch (Exception ex)
                {
                    DataProvider.GameModel.PlayerName = oldPlayerName;
                    DataProvider.SaveGame();
                    PlayerInfoPanelController.Instance?.UpdateInfo();
                    ProfilePanelScreenController.Instance.playerName.text = DataProvider.GameModel.PlayerName;
                    Debug.LogException(ex);
                }

            }

            gameObject.SetActive(false);
            btnLabel.SetActive(true);
            spinner.SetActive(false);
            applyBtn.enabled = true;
        }
    }
}
