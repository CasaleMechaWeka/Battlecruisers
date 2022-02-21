using BattleCruisers.Data;
using BattleCruisers.UI.Loading;
using BattleCruisers.UI.Music;
using BattleCruisers.UI.Sound.AudioSources;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Localisation;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BattleCruisers.PostBattleScreen
{
    public class DestructionRanker : MonoBehaviour
    {
        public GameObject[] destructionRanks;
        [Header("There should be a rank interval for each rank except the very last")]
        public long[] rankIntervals;
        public void DisplayRank(long score)
        {
            destructionRanks[CalculateRank(score)].SetActive(true);
        }

        private int CalculateRank(long score)
        {
            for(int i = 0; i < rankIntervals.Length; i++)
            {
                if (score < rankIntervals[i])
                {
                    return i;
                }
            }
            return 9;
        }

    }

}