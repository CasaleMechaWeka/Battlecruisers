using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Utils.Threading;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test.Balancing
{
    public class KamikazeBalancingTestGod : BuildableVsBuildableTestGod
    {
        private float _cumulativeDelayInS;
        private IDeferrer _deferrer;

        public float scenarioDelayInS;

        // FELIX  Cache Helper in parent class :)
        protected override void InitialiseAsync(Helper parentHelper)
        {
            base.InitialiseAsync(parentHelper);

            Assert.IsTrue(scenarioDelayInS >= 0);

            _deferrer = GetComponent<TimeScaleDeferrer>();
            Assert.IsNotNull(_deferrer);
        }

        protected override void InitialiseScenario(BuildableVsBuildableTest scenario)
        {
            _deferrer.Defer(() => base.InitialiseScenario(scenario), _cumulativeDelayInS);

            _cumulativeDelayInS += scenarioDelayInS;
        }
    }
}
