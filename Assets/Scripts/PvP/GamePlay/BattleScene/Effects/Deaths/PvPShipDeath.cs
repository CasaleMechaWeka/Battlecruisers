using BattleCruisers.Buildables;
using BattleCruisers.Effects;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.ParticleSystems;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Utils.BattleScene.Pools;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Deaths
{
    public class PvPShipDeath : IPoolable<Vector3>
    {
        private readonly PvPMonoBehaviourWrapper _shipDeathController;
        private readonly IBroadcastingAnimation _sinkingAnimation;
        private readonly IList<IPvPParticleSystemGroup> _effects;

        public event EventHandler Deactivated;

        public PvPShipDeath(
            PvPMonoBehaviourWrapper shipDeathController,
            IBroadcastingAnimation sinkingAnimation,
            IList<IPvPParticleSystemGroup> effects)
        {
            PvPHelper.AssertIsNotNull(shipDeathController, sinkingAnimation, effects);

            _shipDeathController = shipDeathController;
            _sinkingAnimation = sinkingAnimation;
            _effects = effects;

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
            /*            foreach (IPvPParticleSystemGroup effect in _effects)
                        {
                            effect.Play();
                        }*/
        }

        private async Task iPlayEffects()
        {
            await Task.Yield();
            foreach (IPvPParticleSystemGroup effect in _effects)
            {
                await effect.Play();
            }
        }

        public void Activate(Vector3 activationArgs, Faction faction)
        {
            // Logging.LogMethod(Tags.DEATHS);

            _shipDeathController.IsVisible = true;
            _shipDeathController.Position = activationArgs;
            Vector3 pos = _shipDeathController.Position;
            if (faction == Faction.Reds)
            {
                Vector3 newScale = _shipDeathController.transform.localScale;
                newScale.x *= -1;
                _shipDeathController.transform.localScale = newScale;
            }

            _sinkingAnimation.Play();

            //     iPlayEffects();                                     // server does not need to play effects.
            /*            foreach (IPvPParticleSystemGroup effect in _effects)
                        {
                            effect.Play();
                        }*/
        }
    }
}