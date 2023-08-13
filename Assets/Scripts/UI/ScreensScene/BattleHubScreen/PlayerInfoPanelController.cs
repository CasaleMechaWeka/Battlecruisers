using BattleCruisers.Data;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.Utils.Fetchers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace BattleCruisers.UI.ScreensScene.BattleHubScreen
{
    public class PlayerInfoPanelController : MonoBehaviour
    {
        public Image _rankImage;
        public Image _captainImage, _selectedCaptainImage;
        public Text _coins, _credits, _playerName;
        public static PlayerInfoPanelController Instance;
        private void Awake()
        {
            Instance = this;
        }
        public void UpdateInfo(IDataProvider dataProvider, IPrefabFactory prefabFactory)
        {
            CaptainExo captain = prefabFactory.GetCaptainExo(dataProvider.GameModel.PlayerLoadout.CurrentCaptain);
            _rankImage.sprite = captain.CaptainExoImage;
            _captainImage.sprite = captain.CaptainExoImage;
            _selectedCaptainImage.sprite = captain.CaptainExoImage;
            _coins.text = dataProvider.GameModel.Coins.ToString();
            _credits.text = dataProvider.GameModel.Credits.ToString(); 
            _playerName.text = dataProvider.GameModel.PlayerName;
        }
    }
}

