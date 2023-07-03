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
        public CaptainExoData[] captainExoDataArray;
        public GameObject uiItemPrefab;
        //public Transform contentTransform;

        private void Start()
        {
            PopulateScrollBar();
        }

        private void PopulateScrollBar()
        {
            foreach (CaptainExoData captainExoData in captainExoDataArray) 
            {
                // Instantiate a new UI Item from the prefab
                GameObject uiItem = Instantiate(uiItemPrefab);

                // Access the UI Item's script and set its data
                CaptainExoItem captainExoItem = uiItem.GetComponentInChildren<CaptainExoItem>();
                captainExoItem.SetCaptainData(captainExoData);
            }
        }
    }
}

