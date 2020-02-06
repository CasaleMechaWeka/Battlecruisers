using System;
using System.Collections;
using System.Collections.Generic;
using BattleCruisers.Effects.Explosions;
using BattleCruisers.Utils.BattleScene.Pools;
using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine;

namespace BattleCruisers.Effects.Deaths
{
    // FELIX  Use, test
    public class ShipDeath : IPoolable<Vector3>
    {
        private readonly IGameObject _shipDeathController;
        private readonly IList<IExplosion> _explosions;
        // FELIX  IBroadcastingAnimation?  Knows when it's done?
        private readonly IAnimation _sinkingAnimation;

        // FELIX  Handle :P
        public event EventHandler Deactivated;

        // FELIX  Constructor

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