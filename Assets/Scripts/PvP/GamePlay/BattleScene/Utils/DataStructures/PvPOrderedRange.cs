using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.DataStrctures
{
    public class PvPOrderedRange : PvPRange<float>
    {
        public PvPOrderedRange(float value1, float value2)
            : base(Mathf.Min(value1, value2), Mathf.Max(value1, value2))
        {
        }
    }
}
