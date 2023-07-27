using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.ParticleSystems;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Deaths
{
    public class PvPShipDeathInitialiser : PvPMonoBehaviourWrapper
    {
        private GameObject effects_parent;
        private PvPShipDeath shipDeath;
        private PvPBroadcastingAnimationController sinkingAnimation;
        private IList<IPvPParticleSystemGroup> effects;
        public IPvPShipDeath CreateShipDeath()
        {
            PvPBroadcastingAnimationController sinkingAnimation = GetComponent<PvPBroadcastingAnimationController>();
            Assert.IsNotNull(sinkingAnimation);

            PvPParticleSystemGroupInitialiser[] particleSystemGroupInitialisers = GetComponentsInChildren<PvPParticleSystemGroupInitialiser>();
            IList<IPvPParticleSystemGroup> effects
                = particleSystemGroupInitialisers
                    .Select(initialiser => initialiser.CreateParticleSystemGroup())
                    .ToList();

            return
                new PvPShipDeath(
                    this,
                    sinkingAnimation,
                    effects);
        }
        protected virtual void Awake()
        {
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
                    sinkingAnimation,
                    effects);
        }
        protected override void SetVisible(bool isVisible)
        {
            //   StartCoroutine(iSetVisible(isVisible));

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
            Position = position;

        }

        [ClientRpc]
        private void SetVisibleClientRpc(bool isVisible)
        {
            IsVisible = isVisible;
            if (IsVisible)
            {
                sinkingAnimation.Play();
                foreach (IPvPParticleSystemGroup effect in effects)
                {
                    effect.Play();
                }
            }
        }

    }
}