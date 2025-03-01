using BattleCruisers.Effects.Explosions;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Explosions;
using UnityEngine.Assertions;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.ParticleSystems
{
    public class PvPParticleSystemGroupInitialiser : PvPMonoBehaviourWrapper, IPvPParticleSystemGroupInitialiser
    {
        public GameObject effects_parent;
        public IPvPParticleSystemGroup CreateParticleSystemGroup()
        {
            return
                new PvPParticleSystemGroup(
                    GetParticleSystems(),
                    GetSynchronizedSystems());
        }

        protected virtual IPvPBroadcastingParticleSystem[] GetParticleSystems()
        {
            PvPBroadcastingParticleSystem[] particleSystems = GetComponentsInChildren<PvPBroadcastingParticleSystem>(includeInactive: true);
            Assert.IsTrue(particleSystems.Length != 0);

            foreach (PvPBroadcastingParticleSystem particleSystem in particleSystems)
            {
                particleSystem.Initialise();
            }

            return particleSystems;
        }

        protected ISynchronizedParticleSystems[] GetSynchronizedSystems()
        {
            PvPSynchronizedParticleSystemsController[] synchronizedSystems = GetComponentsInChildren<PvPSynchronizedParticleSystemsController>();

            foreach (PvPSynchronizedParticleSystemsController system in synchronizedSystems)
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