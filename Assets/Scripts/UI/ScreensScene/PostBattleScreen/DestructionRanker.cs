using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.PostBattleScreen
{
    public class DestructionRanker : MonoBehaviour
    {
        [SerializeField]
        public GameObject[] destructionRanks;
        public void DisplayRank(long score)
        {
            iDisplayRank(score);
        }
        void iDisplayRank(long score)
        {
            foreach (GameObject o in destructionRanks)
                o.SetActive(false);
            int rank = CalculateRank(score);
            Assert.IsTrue(rank >= 0 && rank <= Constants.MAX_RANK);
            if (rank >= 0)
            {
                destructionRanks[rank].SetActive(true);
            }
        }

        public static int CalculateRank(long score)
        {
            for (int i = 0; i <= Constants.MAX_RANK; i++)
            {
                long x = 2500 + 2500 * i * i;
                //Debug.Log(x);
                if (score < x)
                {
                    return i;
                }
            }
            return Constants.MAX_RANK;
        }

        // return what the x value will be in CalculateRank()
        // used for setting the max val of any XP progress bars
        public static long CalculateLevelXP(int i)
        {
            long x = 2500 + 2500 * i * i;
            return x;
        }

        // returns the remainder of the score towards the next level,
        // based on the current lifetime score passed in
        public static long CalculateXpToNextLevel(long score)
        {
            int currentRank = CalculateRank(score); // Calculate the current rank using the existing method

            if (currentRank >= Constants.MAX_RANK)
            {
                // If the current rank is already the highest, there is no remainder
                return 0;
            }

            long currentRankThreshold = 2500 + 2500 * currentRank * currentRank;

            long scoreRemainder = currentRankThreshold - score;
            if (scoreRemainder < 0)
                scoreRemainder = 0;
                
            return scoreRemainder;
        }
    }
}