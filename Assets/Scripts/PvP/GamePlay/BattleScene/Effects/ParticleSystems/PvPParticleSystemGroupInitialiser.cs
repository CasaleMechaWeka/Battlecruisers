using BattleCruisers.Effects.Explosions;
using BattleCruisers.Effects.ParticleSystems;
using UnityEngine.Assertions;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.ParticleSystems
{
    public class PvPParticleSystemGroupInitialiser : PvPMonoBehaviourWrapper, IParticleSystemGroupInitialiser
    {
        public GameObject effects_parent;
        public IParticleSystemGroup CreateParticleSystemGroup()
        {
            return
                new ParticleSystemGroup(
                    GetParticleSystems(),
                    GetSynchronizedSystems());
        }

        protected virtual IBroadcastingParticleSystem[] GetParticleSystems()
        {
            BroadcastingParticleSystem[] particleSystems = GetComponentsInChildren<BroadcastingParticleSystem>(includeInactive: true);
            Assert.IsTrue(particleSystems.Length != 0);

            foreach (BroadcastingParticleSystem particleSystem in particleSystems)
            {
                particleSystem.Initialise();
            }

            return particleSystems;
        }

        protected ISynchronizedParticleSystems[] GetSynchronizedSystems()
        {
            SynchronizedParticleSystemsController[] synchronizedSystems = GetComponentsInChildren<SynchronizedParticleSystemsController>();

            foreach (SynchronizedParticleSystemsController system in synchronizedSystems)
            {
                system.Initialise();
            }
            return synchronizedSystems;
        }

        protected virtual void Awake()
        {
            if (effects_parent == null)
            {
                if (transform.Find("Effects") != null)
                {
                    effects_parent = transform.Find("Effects").gameObject;
                    //    effects_parent.SetActive(false);
                }
            }
        }

        protected override void SetVisible(bool isVisible)
        {
            effects_parent.SetActive(isVisible);
        }
    }
}