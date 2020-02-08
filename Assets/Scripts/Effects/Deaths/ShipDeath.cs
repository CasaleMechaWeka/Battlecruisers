using BattleCruisers.Effects.ParticleSystems;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Effects.Deaths
{
    // FELIX  Use, test
    public class ShipDeath : IShipDeath
    {
        private readonly IGameObject _shipDeathController;
        private readonly IBroadcastingAnimation _sinkingAnimation;
        private IList<IParticleSystemGroup> _effects;

        public event EventHandler Deactivated;

        public ShipDeath(
            IGameObject shipDeathController, 
            IBroadcastingAnimation sinkingAnimation,
            IList<IParticleSystemGroup> effects)
        {
            Helper.AssertIsNotNull(shipDeathController, sinkingAnimation, effects);

            _shipDeathController = shipDeathController;
            _sinkingAnimation = sinkingAnimation;
            _effects = effects;

            // Assume sinking animaion takes longer than other effects.
            _sinkingAnimation.AnimationDone += _sinkingAnimation_AnimationDone;

            _shipDeathController.IsVisible = false;
        }

        private void _sinkingAnimation_AnimationDone(object sender, EventArgs e)
        {
            Logging.LogMethod(Tags.DEATHS);

            _shipDeathController.IsVisible = false;
            Deactivated?.Invoke(this, EventArgs.Empty);
        }

        public void Activate(Vector3 activationArgs)
        {
            Logging.LogMethod(Tags.DEATHS);
            
            _shipDeathController.IsVisible = true;
            _shipDeathController.Position = activationArgs;

            _sinkingAnimation.Play();

            foreach (IParticleSystemGroup effect in _effects)
            {
                effect.Play();
            }
        }
    }
}