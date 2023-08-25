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
using Unity.Services.Authentication;
using Unity.Services.CloudCode;
using System.Linq;

namespace BattleCruisers.UI.ScreensScene.BattleHubScreen
{
    public class LeaderboardPanelScreenController : ScreenController
    {
        const string LeaderboardID = "BC-PvP1v1Leaderboard";

        public LeaderboradPanel TopPlayer;
        [SerializeField]
        private List<LeaderboradPanel> Players;
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
                    //await AuthenticationService.Instance.UpdatePlayerNameAsync(dataProvider.GameModel.PlayerName);
                    await LeaderboardsService.Instance.AddPlayerScoreAsync(LeaderboardID, eol);
                }
                catch(Exception e)
                {
                    e.ToString();
                }

                try
                {
                    var score = await LeaderboardsService.Instance.GetScoresAsync(LeaderboardID);
                    int i = 0;
                    foreach(var entry in score.Results)
                    {
                        if(entry != null)
                        {
                            IList<string> list = entry.PlayerName.Split("#").ToList<string>();
                            Debug.Log(list[1]);
                            if(entry.Rank == 0)
                            {
                                TopPlayer.Initialise(soundPlayer, prefabFactory, list[0], entry.Score, entry.Rank, list[1]);
                            }
                            else
                            {
                                Players[i].Initialise(soundPlayer, prefabFactory, list[0], entry.Score, entry.Rank, list[1]);
                                i++;
                            }
                        }
                        else
                        {
                            return;
                        }
                    }
                    for (int j = i; j < 16; j++)
                    {
                        Players[j].gameObject.SetActive(false);
                    }
                }
                catch(Exception e)
                {
                    e.ToString();
                }

            }
            
            
        }
    }
}