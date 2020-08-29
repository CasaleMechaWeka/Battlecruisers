using BattleCruisers.Effects.Explosions;
using System;
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
            _explosion.Deactivated += _explosion_Deactivated;

            ActivateExplosion();
        }

        private void _explosion_Deactivated(object sender, EventArgs e)
        {
            ActivateExplosion();
        }

        private void ActivateExplosion()
        {
            _explosion.Activate(explosionController.transform.position);
        }
    }
}