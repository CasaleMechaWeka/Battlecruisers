using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Localisation;
using System.Collections.Generic;
using TMPro;

namespace BattleCruisers.UI.ScreensScene.ProfileScreen
{
    public class CaptainItem : MonoBehaviour
    {
        [SerializeField] private Image _captainImage;
        [SerializeField] private string _captainNameText;
        [SerializeField] private int _captainCostText;

        public void SetCaptainData(CaptainData captainData)
        {
            // Assign the captainData values to the UI Item's UI elements
            // You can access the UI elements using GetComponentInChildren, Find, or public references set in the Inspector
            _captainImage.sprite = captainData.CaptainImage;
            _captainNameText = captainData.CaptainName;
            _captainCostText = captainData.CaptainCost;
        }
    }
}

