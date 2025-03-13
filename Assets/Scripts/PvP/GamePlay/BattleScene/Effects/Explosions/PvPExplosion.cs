using BattleCruisers.Buildables;
using BattleCruisers.Effects.Explosions;
using BattleCruisers.Effects.ParticleSystems;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene.Pools;
using BattleCruisers.Utils.PlatformAbstractions;
using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Explosions
{
    public class PvPExplosion : ParticleSystemGroup, IPoolable<Vector3>
    {
        private readonly IGameObject _explosionController;
        private int _systemsCompletedCount = 0;

        public event EventHandler Deactivated;

        public PvPExplosion(IGameObject explosionController, IBroadcastingParticleSystem[] particleSystems, ISynchronizedParticleSystems[] synchronizedSystems)
            : base(particleSystems, synchronizedSystems)
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
            Logging.Verbose(Tags.EXPLOSIONS, $"{_systemsCompletedCount}/{_particleSystems.Length} particle systems completed");
            if (_systemsCompletedCount == _particleSystems.Length)
            {
                Deactivate();
            }
        }

        private void Deactivate()
        {
            Logging.Log(Tags.EXPLOSIONS, _explosionController.ToString());

            _systemsCompletedCount = 0;
            _explosionController.IsVisible = false;
            Deactivated?.Invoke(this, EventArgs.Empty);
        }

        public void Activate(Vector3 position)
        {
            Logging.Log(Tags.EXPLOSIONS, _explosionController.ToString());

            _explosionController.IsVisible = true;
            _explosionController.Position = position;

            //    Play();   // server does not need to play effects  :)
        }

        public void Activate(Vector3 position, Faction faction)
        {
        }
    }
}