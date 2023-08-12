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
        public void DisplayRank(long score)
        {
            for (int i = 0; i < destructionRanks.Length; i++)
            {
                if (i == CalculateRank(score))
                {
                    destructionRanks[i].SetActive(true);
                }
                else
                {
                    destructionRanks[i].SetActive(false);
                }
            }
        }

        public int CalculateRank(long score)
        {
            
            for(int i = 0; i < destructionRanks.Length-1; i++)
            {
                long x = 2500 + 2500*i*i;
                //Debug.Log(x);
                if (score < x)
                {
                    return i;
                }
            }
            return destructionRanks.Length-1;
        }

        // return what the x value will be in CalculateRank()
        // used for setting the max val of any XP progress bars
        public long CalculateLevelXP(int i)
        {
            long x = 2500 + 2500 * i * i;
            return x;
        }

        // returns the remainder of the score towards the next level,
        // based on the current lifetime score passed in
        public long CalculateXpToNextLevel(long score)
        {
            int currentRank = CalculateRank(score); // Calculate the current rank using the existing method

            if (currentRank >= destructionRanks.Length - 1)
            {
                // If the current rank is already the highest, there is no remainder
                return 0;
            }

            long currentRankThreshold = 2500 + 2500 * currentRank * currentRank;
            long nextRankThreshold = 2500 + 2500 * (currentRank + 1) * (currentRank + 1);

            long scoreDifference = nextRankThreshold - currentRankThreshold;
            long scoreRemainder = score - currentRankThreshold;
            if(scoreRemainder < 0)
            {
                scoreRemainder = 0;
            }
            return scoreRemainder;
        }
    }
}