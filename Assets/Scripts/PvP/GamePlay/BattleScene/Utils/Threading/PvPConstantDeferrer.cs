using System;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Threading
{
    public class PvPConstantDeferrer : IPvPConstantDeferrer
    {
        private readonly IPvPDeferrer _deferrer;
        private readonly float _delayInS;

        public PvPConstantDeferrer(IPvPDeferrer deferrer, float delayInS)
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