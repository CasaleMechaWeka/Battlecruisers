using BattleCruisers.Data;
using BattleCruisers.Data.Helpers;
using BattleCruisers.Data.Models;
using BattleCruisers.Scenes;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils.Fetchers;
using UnityEngine;
using UnityEngine.Assertions;
using Unity.Services.Leaderboards;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;

namespace BattleCruisers.UI.ScreensScene.BattleHubScreen
{
    public class LeaderboardPanelScreenController : ScreenController
    {
        const string LeaderboardID = "BC-PvP1v1Leaderboard";

        public GameObject TopPlayer;
        [SerializeField]
        private List<GameObject> Players;
        public async void Initialise(
            IScreensSceneGod screensSceneGod,
            ISingleSoundPlayer soundPlayer,
            IPrefabFactory prefabFactory,
            IDataProvider dataProvider,
            INextLevelHelper nextLevelHelper)
        {
            base.Initialise(screensSceneGod);
            if(Application.internetReachability != NetworkReachability.NotReachable)
            {
                IGameModel gameModel = dataProvider.GameModel;
                double eol = 1200;
                try
                {
                    await LeaderboardsService.Instance.AddPlayerScoreAsync(LeaderboardID, eol);
                }
                catch(Exception e)
                {
                    e.ToString();
                }

                var scoreResponse = LeaderboardsService.Instance.GetScoresAsync(LeaderboardID);
                Debug.Log(JsonConvert.SerializeObject(scoreResponse));

            }
            
            
        }
    }
}