using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Models;
using BattleCruisers.Utils.Fetchers;
using System.Threading.Tasks;
using BattleCruisers.Data.Static;
using System;

namespace BattleCruisers.UI.ScreensScene.ProfileScreen
{
    public class CaptainSelectorPanel : MonoBehaviour
    {
        [SerializeField]
        private Transform buttonContainer;

        [SerializeField]
        private GameObject captainButtonPrefab; // assign this from the editor

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

            foreach (CaptainExoKey captain in StaticPrefabKeys.CaptainExos.AllKeys)
            {
                var captainExoData = await GetCaptainExoData(captain);
/*                if (captainExoData.IsOwned)
                {
                    var buttonGameObject = Instantiate(captainButtonPrefab, buttonContainer);
                    var button = buttonGameObject.GetComponent<CaptainExoButton>();
                    button.Initialize(captain, captainExoData.CaptainExoImage, captain == _gameModel.PlayerLoadout.CurrentCaptain, SelectCaptain);
                }*/
            }
        }

        private async Task<ICaptainExo> GetCaptainExoData(CaptainExoKey captainExoKey)
        {
            IPrefabContainer<ICaptainExo> captainExoPrefabContainer = await _prefabFetcher.GetPrefabAsync<ICaptainExo>(captainExoKey);
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
                button.UpdateActiveState(_gameModel.PlayerLoadout.CurrentCaptain);
            }
        }

        public void SelectCaptain(CaptainExoKey captain)
        {
            _gameModel.PlayerLoadout.CurrentCaptain = captain;
            UpdateActiveCaptain();
            gameObject.SetActive(false); // Close the panel after selection
        }
    }

}
