using BattleCruisers.Effects.Explosions;
using UnityEngine.Assertions;
using BattleCruisers.Utils;
using System.Threading.Tasks;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.ParticleSystems
{
    public class PvPParticleSystemGroup : IPvPParticleSystemGroup
    {
        protected readonly IPvPBroadcastingParticleSystem[] _particleSystems;
        private readonly ISynchronizedParticleSystems[] _synchronizedSystems;

        public PvPParticleSystemGroup(IPvPBroadcastingParticleSystem[] particleSystems, ISynchronizedParticleSystems[] synchronizedSystems)
        {
            Helper.AssertIsNotNull(particleSystems, synchronizedSystems);
            Assert.IsTrue(particleSystems.Length != 0);
            // Synchronized systems may be empty

            _particleSystems = particleSystems;
            _synchronizedSystems = synchronizedSystems;
        }

        public async Task Play()
        {
            await Task.Yield();
            foreach (ISynchronizedParticleSystems system in _synchronizedSystems)
            {
                system.ResetSeed();
            }

            foreach (IPvPBroadcastingParticleSystem particleSystem in _particleSystems)
            {
                particleSystem.Play();
            }
        }

        public async Task Stop()
        {
            await Task.Yield();
            foreach (IPvPBroadcastingParticleSystem particleSystem in _particleSystems)
            {
                particleSystem.Stop();
            }
        }
    }
}