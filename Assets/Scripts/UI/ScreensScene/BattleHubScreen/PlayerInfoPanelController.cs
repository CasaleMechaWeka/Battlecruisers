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
        public void Initialize(Sprite rankSprite, Sprite captainSprite, long coins, long credits, string playerName)
        {
            _rankImage.sprite = rankSprite;
            _captainImage.sprite = captainSprite;
            _selectedCaptainImage.sprite = captainSprite;
            _coins.text = coins.ToString();
            _credits.text = credits.ToString(); 
            _playerName.text = playerName;
        }
    }
}

