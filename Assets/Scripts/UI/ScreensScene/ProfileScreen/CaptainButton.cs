using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using System;
//using BattleCruisers.Utils.Properties;
using UnityEngine;
using TMPro;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Items;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Utils.Properties;

namespace BattleCruisers.UI.ScreensScene.ProfileScreen
{
    public class CaptainButton : MonoBehaviour
    {
        [SerializeField]
        private CaptainData _captainData;
        private IBroadcastingProperty<CaptainData> _selectedCaptain;
        private RectTransform _selectedFeedback;
        public TextMeshProUGUI _unitName;

        public GameObject clickedFeedBack;

        public void Initialise(
            ISingleSoundPlayer soundPlayer,
            CaptainData captainData,
            IBroadcastingProperty<CaptainData> selectedCaptain)
        {
            _captainData = captainData;
            _selectedCaptain = selectedCaptain;

        }
        private void ShowDetails()
        {
            
        }
    }
}

