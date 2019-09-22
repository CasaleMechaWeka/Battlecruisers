using BattleCruisers.Cruisers;
using BattleCruisers.Utils.Threading;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test.Effects.Deaths
{
    public class CruiserDeathTransitionTestGod : CruiserDeathTestGod
    {
        public TimeScaleDeferrer deferrer;
        public float deathTimeInS = 1;
        public GameObject deathPrefab;

        protected override void DestroyCruiser(Cruiser cruiser)
        {
            Assert.IsNotNull(deferrer);
            Assert.IsNotNull(deathPrefab);

            deferrer.Defer(() =>
            {
                Destroy(cruiser.gameObject);

                GameObject deathInstance = Instantiate(deathPrefab);
                deathInstance.transform.position = cruiser.Position;
            }, 
            delayInS: deathTimeInS);
        }
    }
}