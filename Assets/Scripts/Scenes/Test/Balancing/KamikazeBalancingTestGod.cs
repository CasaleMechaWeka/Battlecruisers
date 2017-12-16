using BattleCruisers.Utils;
using BattleCruisers.Utils.Threading;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test.Balancing
{
    public class KamikazeBalancingTestGod : BuildableVsBuildableTestGod
    {
        private float _cumulativeDelayInS;
        private VariableDelayDeferrer _deferrer;

        public float scenarioDelayInS;

        protected override void Initialise()
        {
            base.Initialise();

            Assert.IsTrue(scenarioDelayInS >= 0);

            _deferrer = GetComponent<VariableDelayDeferrer>();
            Assert.IsNotNull(_deferrer);
        }

        protected override void InitialiseScenario(BuildableVsBuildableTest scenario)
        {
            KamikazeBalancingTest kamkazeTest = scenario.Parse<KamikazeBalancingTest>();
            kamkazeTest.StaticInitialise();

            _deferrer.Defer(() => base.InitialiseScenario(scenario), _cumulativeDelayInS);

            _cumulativeDelayInS += scenarioDelayInS;
        }
    }
}
