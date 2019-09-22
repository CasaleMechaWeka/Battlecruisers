using BattleCruisers.Cruisers;
using BattleCruisers.Utils.Threading;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test.Effects.Deaths
{
    public class CruiserDeathTransitionTestGod : CruiserDeathTestGod
    {
        public TimeScaleDeferrer deferrer;
        public float deathTimeInS = 1;

        protected override void DestroyCruiser(Cruiser cruiser)
        {
            Assert.IsNotNull(deferrer);
            deferrer.Defer(() => Destroy(cruiser.gameObject), delayInS: deathTimeInS);
        }
    }
}