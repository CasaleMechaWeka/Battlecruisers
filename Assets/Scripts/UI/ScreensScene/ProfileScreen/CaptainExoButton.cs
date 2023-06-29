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
    public class CaptainExoButton : MonoBehaviour
    {
        [SerializeField]
        private CaptainExoData _captainExoData;
        private IBroadcastingProperty<CaptainExoData> _selectedCaptainExo;
        private RectTransform _selectedFeedback;
        public TextMeshProUGUI _unitName;

        public GameObject clickedFeedBack;

        public void Initialise(
            ISingleSoundPlayer soundPlayer,
            CaptainExoData captainExoData,
            IBroadcastingProperty<CaptainExoData> selectedCaptainExo)
        {
            _captainExoData = captainExoData;
            _selectedCaptainExo = selectedCaptainExo;

        }
        private void ShowDetails()
        {
            
        }
    }
}

