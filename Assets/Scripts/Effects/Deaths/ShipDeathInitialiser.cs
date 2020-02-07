using BattleCruisers.Effects.ParticleSystems;
using BattleCruisers.Utils.BattleScene.Pools;
using BattleCruisers.Utils.PlatformAbstractions;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Effects.Deaths
{
    public class ShipDeathInitialiser : MonoBehaviourWrapper
    {
        public IShipDeath CreateShipDeath()
        {
            BroadcastingAnimationController sinkingAnimation = GetComponent<BroadcastingAnimationController>();
            Assert.IsNotNull(sinkingAnimation);

            ParticleSystemGroupInitialiser[] particleSystemGroupInitialisers = GetComponentsInChildren<ParticleSystemGroupInitialiser>();
            IList<IParticleSystemGroup> effects
                = particleSystemGroupInitialisers
                    .Select(initialiser => initialiser.CreateParticleSystemGroup())
                    .ToList();

            return
                new ShipDeath(
                    new GameObjectBC(gameObject),
                    sinkingAnimation,
                    effects);
        }
    }
}