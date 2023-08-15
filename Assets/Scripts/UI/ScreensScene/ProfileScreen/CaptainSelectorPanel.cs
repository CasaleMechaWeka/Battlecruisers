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
using System.Reflection;
using static BattleCruisers.Data.Static.StaticPrefabKeys;
using BattleCruisers.Data.Models.PrefabKeys;

namespace BattleCruisers.UI.ScreensScene.ProfileScreen
{
    public class CaptainSelectorPanel : MonoBehaviour
    {
        [SerializeField]
        private Transform itemContainer;

        [SerializeField]
        private GameObject captainItemPrefab; // assign this from the editor

        private IDataProvider _dataProvider;
        private IPrefabFactory _prefabFactory;
        private ISingleSoundPlayer _soundPlayer;
        private IScreensSceneGod _screensSceneGod;

        public Transform captainCamContainer;
        public List<GameObject> visualOfCaptains = new List<GameObject>();
        public EventHandler<CaptainDataEventArgs> captainDataChanged;

        public CaptainSelectionItemController currentItem;

        private ICaptainData currentCaptainData;

        public void Initialize(
            IScreensSceneGod screensSceneGod,
            ISingleSoundPlayer soundPlayer,
            IPrefabFactory prefabFactory,
            IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
            _prefabFactory = prefabFactory;
            _soundPlayer = soundPlayer;
            _screensSceneGod = screensSceneGod;
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
        public async void DisplayOwnedCaptains()
        {
            CaptainSelectionItemController[] items = itemContainer.gameObject.GetComponentsInChildren<CaptainSelectionItemController>();
            foreach (CaptainSelectionItemController item in items)
            {
                DestroyImmediate(item.gameObject);
            }

            RemoveAllCaptainsFromRenderCamera();            

            byte ii = 0;
            for (int i = 0; i < StaticPrefabKeys.CaptainExos.AllKeys.Count; i++)
            {
                if (_dataProvider.GameModel.Captains[i].isOwned)
                {
                    
                    GameObject captainItem = Instantiate(captainItemPrefab, itemContainer) as GameObject;
                    CaptainExo captainExo = Instantiate(_prefabFactory.GetCaptainExo(StaticPrefabKeys.CaptainExos.AllKeys[i]), captainCamContainer);
                    captainExo.gameObject.transform.localScale = Vector3.one * 0.5f;
                    captainExo.gameObject.SetActive(false);
                    visualOfCaptains.Add(captainExo.gameObject);
                    captainItem.GetComponent<CaptainSelectionItemController>().StaticInitialise(_soundPlayer, captainExo.CaptainExoImage, _dataProvider.GameModel.Captains[i], this, ii);

                    if (_dataProvider.GameModel.Captains[i].NameStringKeyBase == _dataProvider.GameModel.PlayerLoadout.CurrentCaptain.PrefabName)  // the first item should be clicked :)
                    {
                        captainItem.GetComponent<CaptainSelectionItemController>()._clickedFeedback.SetActive(true);
                        currentItem = captainItem.GetComponent<CaptainSelectionItemController>();
                        captainExo.gameObject.SetActive(true);
                    }
                    ii++;

                    await Task.Delay(10);
                }
            }
        }

        public void ShowCurrentCaptain()
        {
            CaptainExo charlie = Instantiate(_prefabFactory.GetCaptainExo(_dataProvider.GameModel.PlayerLoadout.CurrentCaptain), captainCamContainer);
            charlie.gameObject.transform.localScale = Vector3.one * 0.5f;
            visualOfCaptains.Add(charlie.gameObject);
        }

        private void CaptainDataChanged(object sender, CaptainDataEventArgs e)
        {
            currentItem._clickedFeedback.SetActive(false);
            visualOfCaptains[currentItem._index].SetActive(false);
            currentItem = (CaptainSelectionItemController)sender;
            currentCaptainData = e.captainData;
        }

        public async Task<bool> SaveCurrentItem()
        {
            CaptainExoKey oldExoKey = null;
            if (currentCaptainData != null)
            {
                try
                {
                    oldExoKey = _dataProvider.GameModel.PlayerLoadout.CurrentCaptain;
                    _dataProvider.GameModel.PlayerLoadout.CurrentCaptain = new Data.Models.PrefabKeys.CaptainExoKey(currentCaptainData.NameStringKeyBase);
                    _dataProvider.SaveGame();
                    await _dataProvider.CloudSave();
                    return true;
                }
                catch(Exception ex)
                {
                    _dataProvider.GameModel.PlayerLoadout.CurrentCaptain = oldExoKey;
                    _dataProvider.SaveGame();
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
