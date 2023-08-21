using BattleCruisers.Data;
using BattleCruisers.Scenes;
using BattleCruisers.UI.ScreensScene.BattleHubScreen;
using BattleCruisers.UI.ScreensScene.ShopScreen;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Localisation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene
{
    public class CaptainsContainer : MonoBehaviour
    {
        public Image captainImage;
        public Text captainName;
        public Text captainDescription;
        public Text captainPrice;
        public EventHandler<CaptainDataEventArgs> captainDataChanged;
        private ILocTable commonStrings;
        public CaptainItemController currentItem;
        private ICaptainData currentCaptainData;
        public List<GameObject> visualOfCaptains = new List<GameObject>();
        public GameObject btnBuy, ownFeedback;

        private string firstNameString;
        private string firstDescrtiptionString;

        private ISingleSoundPlayer _soundPlayer;
        private IDataProvider _dataProvider;
        private IPrefabFactory _prefabFactory;
        public GameObject content;

        public void Initialize(ISingleSoundPlayer soundPlayer, IDataProvider dataProvider, IPrefabFactory prefabFactory)
        {
            commonStrings = LandingSceneGod.Instance.commonStrings;
            captainDataChanged += CaptainDataChanged;
            _soundPlayer = soundPlayer;
            _dataProvider = dataProvider;
            _prefabFactory = prefabFactory;
            btnBuy.GetComponent<CanvasGroupButton>().Initialise(_soundPlayer, Purchase);
            firstNameString = captainName.text;
            firstDescrtiptionString = captainDescription.text;
        }

        private async void OnEnable()
        {
            captainName.text = firstNameString;
            captainDescription.text = firstDescrtiptionString;
        }

        private async void Purchase()
        {
            ScreensSceneGod.Instance.processingPanel.SetActive(true);
            if (await LandingSceneGod.CheckForInternetConnection() && AuthenticationService.Instance.IsSignedIn)
            {
                if (_dataProvider.GameModel.Coins >= currentCaptainData.CaptainCost)
                {
                    try
                    {
                        bool result = await _dataProvider.PurchaseCaptain(currentCaptainData.Index);
                        if (result)
                        {

                            await _dataProvider.SyncCurrencyFromCloud();
                            PlayerInfoPanelController.Instance.UpdateInfo(_dataProvider, _prefabFactory);
                            currentItem._clickedFeedback.SetActive(true);
                            currentItem._ownedItemMark.SetActive(true);
                            btnBuy.SetActive(false);
                            ownFeedback.SetActive(true);
                            ScreensSceneGod.Instance.characterOfShop.GetComponent<Animator>().SetTrigger("buy");
                            _dataProvider.GameModel.Captains[currentCaptainData.Index].isOwned = true;
                            _dataProvider.SaveGame();
                            await _dataProvider.CloudSave();
                            ScreensSceneGod.Instance.processingPanel.SetActive(false);
                            MessageBox.Instance.ShowMessage("You got " + commonStrings.GetString(currentCaptainData.NameStringKeyBase));
                        }
                        else
                        {
                            ScreensSceneGod.Instance.processingPanel.SetActive(false);
                            MessageBox.Instance.ShowMessage("Try again later!");
                        }
                    }
                    catch
                    {
                        ScreensSceneGod.Instance.processingPanel.SetActive(false);
                        MessageBox.Instance.ShowMessage("Try again later!");
                    }
                    ScreensSceneGod.Instance.processingPanel.SetActive(false);
                }
                else
                {
                    ScreensSceneGod.Instance.processingPanel.SetActive(false);
                    MessageBox.Instance.ShowMessage("Insufficient Coins!");
                    return;
                }
            }
            else
            {
                ScreensSceneGod.Instance.processingPanel.SetActive(false);
                MessageBox.Instance.ShowMessage("You have no Internet Connection!");
            }
        }

        private void CaptainDataChanged(object sender, CaptainDataEventArgs e)
        {
            currentItem._clickedFeedback.SetActive(false);
            visualOfCaptains[currentItem._index].SetActive(false);
            currentItem = (CaptainItemController)sender;
            currentCaptainData = e.captainData;
            ScreensSceneGod.Instance.characterOfShop.GetComponent<Animator>().SetTrigger("select");
            if (e.captainData.IsOwned)
            {
                btnBuy.SetActive(false);
                ownFeedback.SetActive(true);
            }
            else
            {
                btnBuy.SetActive(true);
                ownFeedback.SetActive(false);
            }

            //    captainImage.sprite = e.captainImage;
            captainName.text = commonStrings.GetString(e.captainData.NameStringKeyBase);
            captainDescription.text = commonStrings.GetString(e.captainData.DescriptionKeyBase);
            captainPrice.text = e.captainData.CaptainCost.ToString();
        }

        private void OnDestroy()
        {
            captainDataChanged -= CaptainDataChanged;
        }
    }

    public class CaptainDataEventArgs : EventArgs
    {
        public ICaptainData captainData { get; set; }
        public Sprite captainImage { get; set; }
    }
}

