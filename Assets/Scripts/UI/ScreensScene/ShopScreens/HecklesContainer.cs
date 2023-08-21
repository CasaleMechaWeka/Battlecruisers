using BattleCruisers.Data;
using BattleCruisers.Scenes;
using BattleCruisers.UI.ScreensScene.BattleHubScreen;
using BattleCruisers.UI.ScreensScene.ShopScreen;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Localisation;
using DG.Tweening;
using System;
using Unity.Services.Authentication;
using Unity.Services.Economy;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene
{
    public class HecklesContainer : MonoBehaviour
    {
        private ILocTable hecklesStrings;
        public EventHandler<HeckleDataEventArgs> heckleDataChanged;

        public Text t_heckleMessage;
        public GameObject obj_heckleMessage;
        public GameObject btnBuy, ownFeedback;
        public HeckleItemController currentItem;
        public IHeckleData currentHeckleData;
        public Text hecklePrice;
        private ISingleSoundPlayer _soundPlayer;
        private IDataProvider _dataProvider;
        private IPrefabFactory _prefabFactory;
        public void Initialize(ISingleSoundPlayer soundPlayer, IDataProvider dataProvider, IPrefabFactory prefabFactory)
        {
            hecklesStrings = LandingSceneGod.Instance.hecklesStrings;
            heckleDataChanged += HeckleDataChanged;
            _soundPlayer = soundPlayer;
            _dataProvider = dataProvider;
            _prefabFactory = prefabFactory;
            btnBuy.GetComponent<CanvasGroupButton>().Initialise(_soundPlayer, Purchase);
        }

        private async void Purchase()
        {
            ScreensSceneGod.Instance.processingPanel.SetActive(true);
            if (await LandingSceneGod.CheckForInternetConnection() && AuthenticationService.Instance.IsSignedIn)
            {
                if (_dataProvider.GameModel.Coins >= currentHeckleData.HeckleCost)
                {
                    try
                    {
                        bool result = await _dataProvider.PurchaseHeckle(currentHeckleData.Index);
                        if (result)
                        {

                            await _dataProvider.SyncCurrencyFromCloud();
                            PlayerInfoPanelController.Instance.UpdateInfo(_dataProvider, _prefabFactory);
                            currentItem._clickedFeedback.SetActive(true);
                            currentItem._ownedItemMark.SetActive(true);
                            btnBuy.SetActive(false);
                            ownFeedback.SetActive(true);
                            ScreensSceneGod.Instance.characterOfShop.GetComponent<Animator>().SetTrigger("buy");
                            _dataProvider.GameModel.Heckles[currentHeckleData.Index].isOwned = true;
                            _dataProvider.SaveGame();
                            await _dataProvider.CloudSave();
                            ScreensSceneGod.Instance.processingPanel.SetActive(false);
                            MessageBox.Instance.ShowMessage("You got \"" + hecklesStrings.GetString(currentHeckleData.StringKeyBase).Substring(0, 10) + "...\"");
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

        private void HeckleDataChanged(object sender, HeckleDataEventArgs e)
        {
            currentItem._clickedFeedback.SetActive(false);
            currentItem = (HeckleItemController)sender;
            currentHeckleData = e.heckleData;
            ScreensSceneGod.Instance.characterOfShop.GetComponent<Animator>().SetTrigger("select");
            if (e.heckleData.IsOwned)
            {
                btnBuy.SetActive(false);
                ownFeedback.SetActive(true);
            }
            else
            {
                btnBuy.SetActive(true);
                ownFeedback.SetActive(false);
            }

            t_heckleMessage.text = hecklesStrings.GetString(e.heckleData.StringKeyBase);
            hecklePrice.text = e.heckleData.HeckleCost.ToString();
            obj_heckleMessage.GetComponent<RectTransform>().localScale = Vector3.zero;
            obj_heckleMessage.GetComponent<RectTransform>().DOScale(Vector3.one, 0.2f);
        }

        private void OnDestroy()
        {
            heckleDataChanged -= HeckleDataChanged;
        }

    }

    public class HeckleDataEventArgs : EventArgs
    {
        public IHeckleData heckleData { get; set; }
    }
}
