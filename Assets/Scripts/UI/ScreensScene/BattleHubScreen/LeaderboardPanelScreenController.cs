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
using UnityEngine.SocialPlatforms.Impl;

namespace BattleCruisers.UI.ScreensScene.BattleHubScreen
{
    public class LeaderboardPanelScreenController : ScreenController
    {
        const string LeaderboardID = "BC-PvP1v1Leaderboard";

        public LeaderboradPanel TopPlayer;
        public GameObject leaderboardPanelPrefab;
        public Transform leaderboardPanelParent;
        public async void Initialise(
            IScreensSceneGod screensSceneGod,
            ISingleSoundPlayer soundPlayer,
            IPrefabFactory prefabFactory,
            IDataProvider dataProvider,
            INextLevelHelper nextLevelHelper)
        {
            base.Initialise(screensSceneGod);
            if (Application.internetReachability != NetworkReachability.NotReachable)
            {
                try
                {
                    var score = await LeaderboardsService.Instance.GetScoresAsync(LeaderboardID);
                    TopPlayer.gameObject.SetActive(false);
                    Transform[] objs = leaderboardPanelParent.GetComponentsInChildren<Transform>();
                    foreach (Transform obj in objs)
                    {
                        if (!ReferenceEquals(obj, leaderboardPanelParent))
                            Destroy(obj.gameObject);
                    }
                    foreach (var entry in score.Results)
                    {
                        if (entry != null)
                        {
                            IList<string> list = entry.PlayerName.Split("#").ToList<string>();
                            if (entry.Rank == 0)
                            {
                                TopPlayer.gameObject.SetActive(true);
                                TopPlayer.Initialise(soundPlayer, prefabFactory, list[0], entry.Score, entry.Rank, list[1]);
                            }
                            else
                            {
                                GameObject panel = Instantiate(leaderboardPanelPrefab, leaderboardPanelParent) as GameObject;
                                panel.GetComponent<LeaderboradPanel>().Initialise(soundPlayer, prefabFactory, list[0], entry.Score, entry.Rank, list[1]);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    e.ToString();
                }
            }
        }
    }
}