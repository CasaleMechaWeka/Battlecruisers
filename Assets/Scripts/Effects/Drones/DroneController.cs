using System;
using BattleCruisers.Buildables;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Localisation;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Effects.Drones
{
    public class DroneController : Prefab, IDroneController
    {
        private Animation _animation;

        public Faction Faction { get; private set; }

        public event EventHandler Activated;
        public event EventHandler Deactivated;

        public override void StaticInitialise(ILocTable commonStrings)
        {
            base.StaticInitialise(commonStrings);

            _animation = GetComponentInChildren<Animation>();
            Assert.IsNotNull(_animation);

            gameObject.SetActive(false);
        }

        public void Activate(DroneActivationArgs activationArgs)
        {
            gameObject.transform.position = activationArgs.Position;
            gameObject.SetActive(true);

            Faction = activationArgs.Faction;

            AnimationState state = _animation["BuilderDrone"];
            Assert.IsNotNull(state);
            state.normalizedTime = RandomGenerator.Instance.Value;
            _animation.Play();

            Activated?.Invoke(this, EventArgs.Empty);
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
            Deactivated?.Invoke(this, EventArgs.Empty);
        }
    }
}