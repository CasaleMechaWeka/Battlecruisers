using UnityEngine;
using UnityEngine.Assertions;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Localisation;
using System.Collections.Generic;
using BattleCruisers.Data;
using System.Collections;
using UnityEngine.UI;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Models;

namespace BattleCruisers.UI.ScreensScene.ProfileScreen
{
    public class CaptainSelectorPanel : MonoBehaviour
    {
        [SerializeField]
        private GameObject buttonPrefab;

        [SerializeField]
        private Transform buttonContainer;

        private IGameModel _gameModel;

        public void Initialize(IGameModel gameModel)
        {
            _gameModel = gameModel;
            PopulateButtons();
        }

        private void PopulateButtons()
        {
            foreach (Transform child in buttonContainer)
            {
                Destroy(child.gameObject);
            }

            foreach (CaptainExoKey captain in _gameModel.UnlockedCaptainExos)
            {
                var button = Instantiate(buttonPrefab, buttonContainer).GetComponent<Button>();
                button.GetComponentInChildren<Text>().text = captain.ToString();
                button.onClick.AddListener(() => _gameModel.CurrentCaptain = captain);

                var captainExoData = GetCaptainExoData(captain);
                button.GetComponentInChildren<Image>().sprite = captainExoData.CaptainExoImage;
            }
        }

        private CaptainExoData GetCaptainExoData(CaptainExoKey captainExoKey)
        {
            // Load the prefab associated with the key
            GameObject captainExoPrefab = Resources.Load<GameObject>(captainExoKey.PrefabPath);

            // Get the CaptainExoData component from the prefab
            CaptainExoData captainExoData = captainExoPrefab.GetComponent<CaptainExoData>();

            // Return the CaptainExoData component
            return captainExoData;
        }

        public void UpdateActiveCaptain()
        {
            foreach (Transform child in buttonContainer)
            {
                var checkbox = child.GetComponentInChildren<Toggle>();
                var button = child.GetComponent<Button>();
                var captainName = button.GetComponentInChildren<Text>().text;
                checkbox.isOn = captainName == _gameModel.CurrentCaptain.ToString();
            }
        }

        private void OnEnable()
        {
            UpdateActiveCaptain();
        }
    }
}