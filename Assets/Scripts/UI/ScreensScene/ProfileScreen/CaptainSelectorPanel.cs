using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Models;
using BattleCruisers.Utils.Fetchers;
using System.Threading.Tasks;
using System;

namespace BattleCruisers.UI.ScreensScene.ProfileScreen
{
    public class CaptainSelectorPanel : MonoBehaviour
    {
        [SerializeField]
        private GameObject buttonPrefab;

        [SerializeField]
        private Transform buttonContainer;

        private IGameModel _gameModel;
        private IPrefabFetcher _prefabFetcher;

        public void Initialize(IGameModel gameModel, IPrefabFetcher prefabFetcher)
        {
            _gameModel = gameModel;
            _prefabFetcher = prefabFetcher;
            PopulateButtons();
        }

        private async void PopulateButtons()
        {
            foreach (Transform child in buttonContainer)
            {
                Destroy(child.gameObject);
            }

            foreach (CaptainExoKey captain in _gameModel.UnlockedCaptainExos)
            {
                var button = Instantiate(buttonPrefab, buttonContainer).GetComponent<CaptainExoButton>();

                var captainExoData = await GetCaptainExoData(captain);

                button.Initialize(captain, captainExoData.CaptainExoImage, captain == _gameModel.CurrentCaptain);
            }
        }

        private async Task<ICaptainExoData> GetCaptainExoData(CaptainExoKey captainExoKey)
        {
            // Use the PrefabFetcher to load the prefab associated with the key
            IPrefabContainer<ICaptainExoData> captainExoPrefabContainer = await _prefabFetcher.GetPrefabAsync<ICaptainExoData>(captainExoKey);

            // Return the CaptainExoData component
            return captainExoPrefabContainer.Prefab;
        }

        private void OnEnable()
        {
            UpdateActiveCaptain();
        }

        private void UpdateActiveCaptain()
        {
            foreach (Transform child in buttonContainer)
            {
                var button = child.GetComponent<CaptainExoButton>();
                button.SetActiveCaptain(_gameModel.CurrentCaptain);
            }
        }

    }
}
