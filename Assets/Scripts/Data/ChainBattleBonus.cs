using UnityEngine;
using BattleCruisers.Buildables.Boost.GlobalProviders;

namespace BattleCruisers.Data
{
    [System.Serializable]
    public class ChainBattleBonus
    {
        public string bonusName;
        public string description;
        public string engagedMessage;
        public BoostType type;
        public float value;
    }
}
