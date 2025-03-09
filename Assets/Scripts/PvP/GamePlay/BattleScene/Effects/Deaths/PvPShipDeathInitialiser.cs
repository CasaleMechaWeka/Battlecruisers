using BattleCruisers.Effects.ParticleSystems;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.ParticleSystems;
using BattleCruisers.Utils.BattleScene.Pools;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Deaths
{
    public class PvPShipDeathInitialiser : PvPMonoBehaviourWrapper
    {
        public GameObject effects_parent;
        private PvPShipDeath shipDeath;
        private PvPBroadcastingAnimationController sinkingAnimation;
        private IList<IParticleSystemGroup> effects;
        public IPoolable<Vector3> CreateShipDeath()
        {
            PvPBroadcastingAnimationController sinkingAnimation = GetComponent<PvPBroadcastingAnimationController>();
            Assert.IsNotNull(sinkingAnimation);

            PvPParticleSystemGroupInitialiser[] particleSystemGroupInitialisers = GetComponentsInChildren<PvPParticleSystemGroupInitialiser>();
            IList<IParticleSystemGroup> effects
                = particleSystemGroupInitialisers
                    .Select(initialiser => initialiser.CreateParticleSystemGroup())
                    .ToList();

            return
                new PvPShipDeath(
                    this,
                    sinkingAnimation);
        }
        protected virtual void Awake()
        {
            if (effects_parent == null)
                effects_parent = transform.Find("Effects").gameObject;
            sinkingAnimation = GetComponent<PvPBroadcastingAnimationController>();
            Assert.IsNotNull(sinkingAnimation);

            PvPParticleSystemGroupInitialiser[] particleSystemGroupInitialisers = GetComponentsInChildren<PvPParticleSystemGroupInitialiser>();
            effects
                = particleSystemGroupInitialisers
                    .Select(initialiser => initialiser.CreateParticleSystemGroup())
                    .ToList();

            shipDeath =
                new PvPShipDeath(
                    this,
                    sinkingAnimation);
        }
        protected override void SetVisible(bool isVisible)
        {
            //   StartCoroutine(iSetVisible(isVisible));
            if (effects_parent != null)
                effects_parent.SetActive(isVisible);
        }

        protected override void CallRpc_SetVisible(bool isVisible)
        {
            SetVisibleClientRpc(isVisible);
        }
        protected override void CallRpc_SetPosition(Vector3 position)
        {
            SetPositionClientRpc(position);
        }

        [ClientRpc]
        private void SetPositionClientRpc(Vector3 position)
        {
            if (!IsHost)
                Position = position;
        }

        [ClientRpc]
        private void SetVisibleClientRpc(bool isVisible)
        {
            if (!IsHost)
            {
                IsVisible = isVisible;
            }
            if (isVisible)
            {
                sinkingAnimation.Play();
                foreach (IParticleSystemGroup effect in effects)
                    effect.Play();
            }
        }
    }
}