using System;
using System.Collections;
using System.Collections.Generic;
using BattleCruisers.Effects.Explosions;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene.Pools;
using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine;

namespace BattleCruisers.Effects.Deaths
{
    // FELIX  Use, test
    public class ShipDeath : IPoolable<Vector3>
    {
        private readonly IGameObject _shipDeathController;
        // FELIX  Add explosions!
        //private readonly IList<IExplosion> _explosions;
        private readonly IBroadcastingAnimation _sinkingAnimation;

        public event EventHandler Deactivated;

        public ShipDeath(IGameObject shipDeathController, IBroadcastingAnimation sinkingAnimation)
        {
            Helper.AssertIsNotNull(shipDeathController, sinkingAnimation);

            _shipDeathController = shipDeathController;
            _sinkingAnimation = sinkingAnimation;

            // Assume sinking animaion takes longer than other effects.
            _sinkingAnimation.AnimationDone += _sinkingAnimation_AnimationDone;
        }

        private void _sinkingAnimation_AnimationDone(object sender, EventArgs e)
        {
            _shipDeathController.IsVisible = false;
            Deactivated?.Invoke(this, EventArgs.Empty);
        }

        public void Activate(Vector3 activationArgs)
        {
            _shipDeathController.IsVisible = true;
            _shipDeathController.Position = activationArgs;

            _sinkingAnimation.Play();

            // FELIX  Explosions are abstracted...  Don't expose position :(  => Use existing pools?
            //foreach (IExplosion explosion in _explosions)
            //{
            //    explosion.Activate(explosion.)
            //}
        }
    }
}