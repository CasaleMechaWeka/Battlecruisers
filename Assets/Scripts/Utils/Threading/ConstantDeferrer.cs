using System;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils.Threading
{
    public class ConstantDeferrer : IConstantDeferrer
    {
        private readonly IDeferrer _deferrer;
        private readonly float _delayInS;

        public ConstantDeferrer(IDeferrer deferrer, float delayInS)
        {
            Assert.IsNotNull(deferrer);

            _deferrer = deferrer;
            _delayInS = delayInS;
        }

        public void Defer(Action action)
        {
            _deferrer.Defer(action, _delayInS);
        }
    }
}