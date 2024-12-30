using BattleCruisers.Utils.DataStrctures;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Clamping
{
    public class PvPBufferClamper : IPvPClamper
    {
        private readonly float _buffer;

        public PvPBufferClamper(float buffer)
        {
            _buffer = buffer;
        }

        public float Clamp(float value, IRange<float> validRange)
        {
            return
                Mathf.Clamp(
                    value,
                    validRange.Min - _buffer,
                    validRange.Max + _buffer);
        }
    }
}