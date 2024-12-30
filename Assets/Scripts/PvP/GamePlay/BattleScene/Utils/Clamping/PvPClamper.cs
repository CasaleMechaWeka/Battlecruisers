using BattleCruisers.Utils.DataStrctures;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Clamping
{
    public class PvPClamper : IPvPClamper
    {
        public float Clamp(float value, IRange<float> validRange)
        {
            return Mathf.Clamp(value, validRange.Min, validRange.Max);
        }
    }
}