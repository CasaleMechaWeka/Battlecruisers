using BattleCruisers.Utils.DataStrctures;
using UnityEngine;

namespace BattleCruisers.Utils.Clamping
{
    public class BufferClamper : IClamper
    {
        private readonly float _buffer;

        public BufferClamper(float buffer)
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