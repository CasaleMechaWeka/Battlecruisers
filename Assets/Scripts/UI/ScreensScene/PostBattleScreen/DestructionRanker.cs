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
            destructionRanks[CalculateRank(score)].SetActive(true);
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
        // based on the rank and current lifetime score passed in
        //public long CalculateXpToNextLevel(int rank, long lifetimeScore)
        //{
        //    for (int i = 0; i < rank - 1; i++)
        //    {
        //        lifetimeScore -= CalculateLevelXP(i);
        //    }
        //    return lifetimeScore;
        //}
    }
}