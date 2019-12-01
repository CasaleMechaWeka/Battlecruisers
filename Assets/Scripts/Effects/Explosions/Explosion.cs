using BattleCruisers.Effects.ParticleSystems;
using BattleCruisers.Utils.PlatformAbstractions;
using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Effects.Explosions
{
    public class Explosion : ParticleSystemGroup, IExplosion
    {
        private readonly IGameObject _explosionController;
        private int _systemsCompletedCount = 0;

        public event EventHandler Deactivated;

        public Explosion(IGameObject explosionController, IBroadcastingParticleSystem[] particleSystems)
            : base(particleSystems)
        {
            Assert.IsNotNull(explosionController);
            _explosionController = explosionController;

            foreach (IBroadcastingParticleSystem particleSystem in _particleSystems)
            {
                particleSystem.Stopped += ParticleSystem_Stopped;
            }

            _explosionController.IsVisible = false;
        }

        private void ParticleSystem_Stopped(object sender, EventArgs e)
        {
            _systemsCompletedCount++;

            if (_systemsCompletedCount == _particleSystems.Length)
            {
                _systemsCompletedCount = 0;
                _explosionController.IsVisible = false;
                Deactivated?.Invoke(this, EventArgs.Empty);
            }
        }

        public void Activate(Vector3 position)
        {
            _explosionController.IsVisible = true;
            _explosionController.Position = position;

            Play();
        }
    }
}