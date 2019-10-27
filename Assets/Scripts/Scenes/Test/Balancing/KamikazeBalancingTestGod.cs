using BattleCruisers.Utils.Threading;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test.Balancing
{
    public class KamikazeBalancingTestGod : BuildableVsBuildableTestGod
    {
        private float _cumulativeDelayInS;
        private IDeferrer _deferrer;

        public float scenarioDelayInS;

        protected override IList<GameObject> GetGameObjects()
        {
            Assert.IsTrue(scenarioDelayInS >= 0);

            _deferrer = GetComponent<TimeScaleDeferrer>();
            Assert.IsNotNull(_deferrer);

            return base.GetGameObjects();
        }

        protected override void InitialiseScenario(BuildableVsBuildableTest scenario)
        {
            _deferrer.Defer(() => base.InitialiseScenario(scenario), _cumulativeDelayInS);
            _cumulativeDelayInS += scenarioDelayInS;
        }
    }
}
