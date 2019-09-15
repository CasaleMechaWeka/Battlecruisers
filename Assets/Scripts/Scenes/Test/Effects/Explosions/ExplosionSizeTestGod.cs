using BattleCruisers.Effects.Explosions;
using BattleCruisers.Utils.Threading;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test.Effects.Explosions
{
    public class ExplosionSizeTestGod : MonoBehaviour
    {
        // FELIX
        void Start()
        {
            TimeScaleDeferrer deferrer = GetComponent<TimeScaleDeferrer>();
            Assert.IsNotNull(deferrer);

            ExplosionController[] explosionControllers = GetComponentsInChildren<ExplosionController>();

            foreach (ExplosionController explosionController in explosionControllers)
            {
                IExplosion explosion = explosionController.Initialise();
                explosion.Deactivated += (sender, e) => deferrer.Defer(() => explosion.Activate(explosionController.Position), delayInS: 0.25f);
                explosion.Activate(explosionController.Position);
            }

            //AdvancedExplosion[] explosions = GetComponentsInChildren<AdvancedExplosion>();

            //foreach (AdvancedExplosion explosion in explosions)
            //{
            //    explosion.Initialise(RandomGenerator.Instance);
            //    explosion.Activate(explosion.transform.position);
            //}
        }
    }
}