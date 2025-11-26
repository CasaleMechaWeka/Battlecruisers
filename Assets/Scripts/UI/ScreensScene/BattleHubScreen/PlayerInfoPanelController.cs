using BattleCruisers.Data;
using BattleCruisers.PostBattleScreen;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Fetchers.Sprites;
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
        public async void UpdateInfo()
        {
            CaptainExo captain = PrefabFactory.GetCaptainExo(DataProvider.GameModel.PlayerLoadout.CurrentCaptain);

            _captainImage.sprite = captain.CaptainExoImage;
            _selectedCaptainImage.sprite = captain.CaptainExoImage;
            _coins.text = DataProvider.GameModel.Coins.ToString();
            _credits.text = DataProvider.GameModel.Credits.ToString();
            _playerName.text = DataProvider.GameModel.PlayerName;

            int rank = DestructionRanker.CalculateRank(DataProvider.GameModel.LifetimeDestructionScore);
            _rankImage.sprite = await SpriteFetcher.GetSpriteAsync($"{SpritePaths.RankImagesPath}Rank{rank}.png");
        }
    }
}

