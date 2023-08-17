using BattleCruisers.Data;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Fetchers.Sprites;
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
        public async void UpdateInfo(IDataProvider dataProvider, IPrefabFactory prefabFactory)
        {
            CaptainExo captain = prefabFactory.GetCaptainExo(dataProvider.GameModel.PlayerLoadout.CurrentCaptain);

            _captainImage.sprite = captain.CaptainExoImage;
            _selectedCaptainImage.sprite = captain.CaptainExoImage;
            _coins.text = dataProvider.GameModel.Coins.ToString();
            _credits.text = dataProvider.GameModel.Credits.ToString(); 
            _playerName.text = dataProvider.GameModel.PlayerName;

            int rank = CalculateRank(dataProvider.GameModel.LifetimeDestructionScore);
            SpriteFetcher fetcher = new SpriteFetcher();
            _rankImage.sprite = (await fetcher.GetSpriteAsync("Assets/Resources_moved/Sprites/UI/ScreensScene/DestructionScore/" + StaticPrefabKeys.Ranks.AllRanks[rank].RankImage + ".png")).Sprite;
        }

        private int CalculateRank(long score)
        {

            for (int i = 0; i <= StaticPrefabKeys.Ranks.AllRanks.Count; i++)
            {
                long x = 2500 + 2500 * i * i;
                //Debug.Log(x);
                if (score < x)
                {
                    return i;
                }
            }
            return StaticPrefabKeys.Ranks.AllRanks.Count;
        }
    }
}

