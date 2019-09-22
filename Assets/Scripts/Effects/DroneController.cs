using System;
using BattleCruisers.Buildables;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Effects
{
    public class DroneController : MonoBehaviour, IDroneController
    {
        private IRandomGenerator _random;
        private Animation _animation;
        private AudioSource _audioSource;

        public Faction Faction { get; private set; }

        public event EventHandler Activated;
        public event EventHandler Deactivated;

        public void Initialise(IRandomGenerator random)
        {
            Assert.IsNotNull(random);
            _random = random;

            _animation = GetComponentInChildren<Animation>();
            Assert.IsNotNull(_animation);

            _audioSource = GetComponentInChildren<AudioSource>();
            Assert.IsNotNull(_audioSource);

            gameObject.SetActive(false);
        }

        public void Activate(DroneActivationArgs activationArgs)
        {
            gameObject.transform.position = activationArgs.Position;
            gameObject.SetActive(true);

            _audioSource.gameObject.SetActive(activationArgs.PlayAudio);

            Faction = activationArgs.Faction;

            AnimationState state = _animation["BuilderDrone"];
            Assert.IsNotNull(state);
            state.normalizedTime = _random.Value;
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