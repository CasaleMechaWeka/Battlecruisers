using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions;
using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Effects.Explosions
{
    public class Explosion : IExplosion
    {
        private readonly IGameObject _explosionController;
        private readonly IBroadcastingParticleSystem[] _particleSystems;
        private int _systemsCompletedCount = 0;

        public event EventHandler Deactivated;

        public Explosion(IGameObject explosionController, IBroadcastingParticleSystem[] particleSystems)
        {
            Helper.AssertIsNotNull(explosionController, particleSystems);
            Assert.IsTrue(particleSystems.Length != 0);

            _explosionController = explosionController;
            _particleSystems = particleSystems;

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

            foreach (IBroadcastingParticleSystem particleSystem in _particleSystems)
            {
                particleSystem.Play();
            }
        }
    }
}