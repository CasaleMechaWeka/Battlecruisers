using BattleCruisers.Effects.Explosions;
using BattleCruisers.Utils.Threading;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test.Effects.Explosions
{
    public class ExplosionSizeTestGod : MonoBehaviour
    {
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
        }
    }
}