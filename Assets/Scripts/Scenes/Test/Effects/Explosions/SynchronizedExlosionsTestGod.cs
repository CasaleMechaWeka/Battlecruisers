using BattleCruisers.Effects.Explosions;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test.Effects.Explosions
{
    public class SynchronizedExlosionsTestGod : MonoBehaviour
    {
        private IExplosion _explosion;

        public ExplosionController explosionController;

        void Start()
        {
            Assert.IsNotNull(explosionController);
            _explosion = explosionController.Initialise();

            InvokeRepeating(nameof(ActivateExplosion), time: 0, repeatRate: 3.1f);
        }

        private void ActivateExplosion()
        {
            _explosion.Activate(explosionController.transform.position);
        }
    }
}