using UnityEngine;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Data;
using BattleCruisers.Scenes;
using BattleCruisers.UI.Sound.Players;
using System.Threading.Tasks;
using System.Collections.Generic;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.ScreensScene.ShopScreen;
using System;
using BattleCruisers.Data.Models.PrefabKeys;
using Unity.Services.Authentication;

namespace BattleCruisers.UI.ScreensScene.ProfileScreen
{
    public class CaptainSelectorPanel : MonoBehaviour
    {
        [SerializeField]
        private Transform itemContainer;

        [SerializeField]
        private GameObject captainItemPrefab; // assign this from the editor

        private PrefabFactory _prefabFactory;
        private ISingleSoundPlayer _soundPlayer;

        public Transform captainCamContainer;
        public List<GameObject> visualOfCaptains = new List<GameObject>();
        public EventHandler<CaptainDataEventArgs> captainDataChanged;

        public CaptainSelectionItemController currentItem;

        private ICaptainData currentCaptainData;

        public void Initialize(
            ISingleSoundPlayer soundPlayer,
            PrefabFactory prefabFactory)
        {
            _prefabFactory = prefabFactory;
            _soundPlayer = soundPlayer;
            captainDataChanged += CaptainDataChanged;
        }

        public void RemoveAllCaptainsFromRenderCamera()
        {
            foreach (GameObject obj in visualOfCaptains)
            {
                if (obj != null)
                    DestroyImmediate(obj);
            }
            visualOfCaptains.Clear();
        }
        public void DisplayOwnedCaptains()
        {
            CaptainSelectionItemController[] items = itemContainer.gameObject.GetComponentsInChildren<CaptainSelectionItemController>();
            foreach (CaptainSelectionItemController item in items)
            {
                Destroy(item.gameObject);
            }

            RemoveAllCaptainsFromRenderCamera();

            byte ii = 0;
            for (int i = 0; i < StaticPrefabKeys.CaptainExos.CaptainExoCount; i++)
            {
                if (DataProvider.GameModel.PurchasedExos.Contains(i))
                {

                    GameObject captainItem = Instantiate(captainItemPrefab, itemContainer) as GameObject;
                    CaptainExo captainExoPrefab = _prefabFactory.GetCaptainExo(StaticPrefabKeys.CaptainExos.GetCaptainExoKey(i));
                    CaptainExo captainExo = Instantiate(captainExoPrefab, captainCamContainer);
                    captainExo.gameObject.transform.localScale = Vector3.one * 0.5f;
                    captainExo.gameObject.SetActive(false);
                    visualOfCaptains.Add(captainExo.gameObject);
                    captainItem.GetComponent<CaptainSelectionItemController>().StaticInitialise(_soundPlayer, captainExo.CaptainExoImage, StaticData.Captains[i], this, ii);

                    if (StaticData.Captains[i].NameStringKeyBase == DataProvider.GameModel.PlayerLoadout.CurrentCaptain.PrefabName)  // the first item should be clicked :)
                    {
                        captainItem.GetComponent<CaptainSelectionItemController>()._clickedFeedback.SetActive(true);
                        currentItem = captainItem.GetComponent<CaptainSelectionItemController>();
                        captainExo.gameObject.SetActive(true);
                    }
                    ii++;

                }
            }
        }

        public void ShowCurrentCaptain()
        {
            CaptainExo charliePrefab = _prefabFactory.GetCaptainExo(DataProvider.GameModel.PlayerLoadout.CurrentCaptain);
            CaptainExo charlie = Instantiate(charliePrefab, captainCamContainer);
            charlie.gameObject.transform.localScale = Vector3.one * 0.5f;
            visualOfCaptains.Add(charlie.gameObject);
        }

        private void CaptainDataChanged(object sender, CaptainDataEventArgs e)
        {
            if (currentItem != null)
            {
                currentItem._clickedFeedback.SetActive(false);
                visualOfCaptains[currentItem._index].SetActive(false);
                currentItem.CaptainName.gameObject.SetActive(false);
            }
            else
            {
                foreach (GameObject obj in visualOfCaptains)
                {
                    obj.SetActive(false);
                }
            }
            currentItem = (CaptainSelectionItemController)sender;
            currentCaptainData = e.captainData;
            currentItem.CaptainName.gameObject.SetActive(true);
        }

        public async Task<bool> SaveCurrentItem()
        {
            CaptainExoKey oldExoKey = null;
            if (currentCaptainData != null)
            {
                try
                {
                    oldExoKey = DataProvider.GameModel.PlayerLoadout.CurrentCaptain;
                    DataProvider.GameModel.PlayerLoadout.CurrentCaptain = new CaptainExoKey(currentCaptainData.NameStringKeyBase);
                    DataProvider.SaveGame();

                    // online functions
                    if (await LandingSceneGod.CheckForInternetConnection() && AuthenticationService.Instance.IsSignedIn)
                    {
                        await DataProvider.CloudSave();
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    DataProvider.GameModel.PlayerLoadout.CurrentCaptain = oldExoKey;
                    DataProvider.SaveGame();
                    Debug.LogException(ex);
                    return false;
                }

            }
            return false;
        }
        private void OnDestroy()
        {
            captainDataChanged -= CaptainDataChanged;
        }
    }
}
