using UnityEngine;
using UnityEngine.Assertions;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Localisation;
using System.Collections.Generic;

namespace BattleCruisers.UI.ScreensScene.ProfileScreen
{
    public class InfiniteCaptainScreenController : MonoBehaviour
    {
        public CaptainData[] captainDataArray;
        public GameObject uiItemPrefab;
        //public Transform contentTransform;

        private void Start()
        {
            PopulateScrollBar();
        }

        private void PopulateScrollBar()
        {
            foreach (CaptainData captainData in captainDataArray) 
            {
                // Instantiate a new UI Item from the prefab
                GameObject uiItem = Instantiate(uiItemPrefab);

                // Access the UI Item's script and set its data
                CaptainItem captainItem = uiItem.GetComponentInChildren<CaptainItem>();
                captainItem.SetCaptainData(captainData);
            }
        }
    }
}

