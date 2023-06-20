using System;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Utils.Localisation;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Drones
{
    public class PvPDroneController : PvPPrefab, IPvPDroneController
    {
        private Animation _animation;

        public PvPFaction Faction { get; private set; }

        public event EventHandler Activated;
        public event EventHandler Deactivated;

        public override void StaticInitialise(ILocTable commonStrings)
        {
            base.StaticInitialise(commonStrings);

            _animation = GetComponentInChildren<Animation>();
            Assert.IsNotNull(_animation);

            gameObject.SetActive(false);
        }

        public void Activate(PvPDroneActivationArgs activationArgs)
        {         

            gameObject.transform.position = activationArgs.Position;
            gameObject.SetActive(true);

            Faction = activationArgs.Faction;

            AnimationState state = _animation["BuilderDrone"];
            Assert.IsNotNull(state);
            state.normalizedTime = PvPRandomGenerator.Instance.Value;
            _animation.Play();

            Activated?.Invoke(this, EventArgs.Empty);
        }

        public void Activate(PvPDroneActivationArgs activationArgs, PvPFaction faction)
        {
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
            Deactivated?.Invoke(this, EventArgs.Empty);
        }
    }
}