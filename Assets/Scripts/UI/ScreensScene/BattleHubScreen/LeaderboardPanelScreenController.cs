using BattleCruisers.Scenes;
using UnityEngine;
using Unity.Services.Leaderboards;
using System.Collections.Generic;
using System;
using System.Linq;

namespace BattleCruisers.UI.ScreensScene.BattleHubScreen
{
    public class LeaderboardPanelScreenController : ScreenController
    {
        const string LeaderboardID = "BC-PvP1v1Leaderboard";

        public LeaderboradPanel TopPlayer;
        public GameObject leaderboardPanelPrefab;
        public Transform leaderboardPanelParent;
        public GameObject noData;
        public new async void Initialise(ScreensSceneGod screensSceneGod)
        {
            base.Initialise(screensSceneGod);
            noData.SetActive(false);
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

                    if (score.Results.Count == 0)
                        noData.SetActive(true);
                    int i = 0;
                    foreach (var entry in score.Results)
                    {
                        if (i >= 20)
                            break;
                        if (entry != null)
                        {
                            IList<string> list = entry.PlayerName.Split("#").ToList();
                            if (entry.Rank == 0)
                            {
                                TopPlayer.gameObject.SetActive(true);
                                TopPlayer.Initialise(list[0], entry.Score, entry.Rank, list[1]);
                            }
                            else
                            {
                                GameObject panel = Instantiate(leaderboardPanelPrefab, leaderboardPanelParent);
                                panel.GetComponent<LeaderboradPanel>().Initialise(list[0], entry.Score, entry.Rank, list[1]);
                            }
                            i++;
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