using BattleCruisers.Buildables;
using BattleCruisers.Effects;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Utils.BattleScene.Pools;
using System;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Deaths
{
    public class PvPShipDeath : IPoolable<Vector3>
    {
        private readonly PvPMonoBehaviourWrapper _shipDeathController;
        private readonly IBroadcastingAnimation _sinkingAnimation;

        public event EventHandler Deactivated;

        public PvPShipDeath(
            PvPMonoBehaviourWrapper shipDeathController,
            IBroadcastingAnimation sinkingAnimation)
        {
            PvPHelper.AssertIsNotNull(shipDeathController, sinkingAnimation);

            _shipDeathController = shipDeathController;
            _sinkingAnimation = sinkingAnimation;

            // Assume sinking animaion takes longer than other effects.
            _sinkingAnimation.AnimationDone += _sinkingAnimation_AnimationDone;

            _shipDeathController.IsVisible = false;
        }

        private void _sinkingAnimation_AnimationDone(object sender, EventArgs e)
        {
            // Logging.LogMethod(Tags.DEATHS);

            _shipDeathController.IsVisible = false;
            Deactivated?.Invoke(this, EventArgs.Empty);
        }

        public void Activate(Vector3 activationArgs)
        {
            // Logging.LogMethod(Tags.DEATHS);

            _shipDeathController.IsVisible = true;
            _shipDeathController.Position = activationArgs;


            //    _sinkingAnimation.Play();                          // server does not need to play effects
            //    iPlayEffects();
            /*            foreach (IParticleSystemGroup effect in _effects)
                        {
                            effect.Play();
                        }*/
        }

        public void Activate(Vector3 activationArgs, Faction faction)
        {
            // Logging.LogMethod(Tags.DEATHS);

            _shipDeathController.IsVisible = true;
            _shipDeathController.Position = activationArgs;
            if (faction == Faction.Reds)
            {
                Vector3 newScale = _shipDeathController.transform.localScale;
                newScale.x *= -1;
                _shipDeathController.transform.localScale = newScale;
            }

            _sinkingAnimation.Play();

            //     iPlayEffects();                                     // server does not need to play effects.
            /*            foreach (IParticleSystemGroup effect in _effects)
                        {
                            effect.Play();
                        }*/
        }
    }
}