using BattleCruisers.Buildables;
using BattleCruisers.Cruisers.Damage;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Effects.Smoke
{
    public class SmokeInitialiser : MonoBehaviour
    {
        // Keep reference to avoid garbage collection
#pragma warning disable CS0414  // Variable is assigned but never used
        private SmokeEmitter _smokeEmitter;
#pragma warning restore CS0414  // Variable is assigned but never used

        public void Initialise(IDamagable parentDamagable, bool showSmokeWhenDestroyed)
        {
            Smoke smoke = GetComponent<Smoke>();
            Assert.IsNotNull(smoke);
            smoke.Initialise();

            _smokeEmitter
                = new SmokeEmitter(
                    new HealthStateMonitor(parentDamagable),
                    smoke,
                    showSmokeWhenDestroyed);
        }
    }
}