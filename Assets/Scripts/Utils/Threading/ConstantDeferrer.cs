using System;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils.Threading
{
    public class ConstantDeferrer : IConstantDeferrer
    {
        private readonly IDeferrer _deferrer;
        private readonly float _delayInMs;

        public ConstantDeferrer(IDeferrer deferrer, float delayInMs)
        {
            Assert.IsNotNull(deferrer);

            _deferrer = deferrer;
            _delayInMs = delayInMs;
        }

        public void Defer(Action action)
        {
            _deferrer.Defer(action, _delayInMs);
        }
    }
}