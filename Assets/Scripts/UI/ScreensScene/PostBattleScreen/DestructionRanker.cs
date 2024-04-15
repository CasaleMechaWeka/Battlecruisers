using BattleCruisers.Data.Static;
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
            Assert.IsTrue(rank >= 0 && rank <= StaticPrefabKeys.Ranks.AllRanks.Count - 1);
            if (rank >= 0)
            {
                destructionRanks[rank].SetActive(true);
            }
        }

        public int CalculateRank(long score)
        {

            for (int i = 0; i <= StaticPrefabKeys.Ranks.AllRanks.Count - 1; i++)
            {
                long x = 2500 + 2500 * i * i;
                //Debug.Log(x);
                if (score < x)
                {
                    return i;
                }
            }
            return StaticPrefabKeys.Ranks.AllRanks.Count - 1;
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

            if (currentRank >= StaticPrefabKeys.Ranks.AllRanks.Count - 1)
            {
                // If the current rank is already the highest, there is no remainder
                return 0;
            }

            long currentRankThreshold = 2500 + 2500 * currentRank * currentRank;
            long nextRankThreshold = 2500 + 2500 * (currentRank + 1) * (currentRank + 1);

            long scoreDifference = nextRankThreshold - currentRankThreshold;
            long scoreRemainder = currentRankThreshold - score;
            if (scoreRemainder < 0)
            {
                scoreRemainder = 0;
            }
            return scoreRemainder;
        }
    }
}