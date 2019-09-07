using System;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Effects
{
    public class DroneController : MonoBehaviour, IDroneController
    {
        private IRandomGenerator _random;
        private Animation _animation;

        public event EventHandler Deactivated;

        public void Initialise(IRandomGenerator random)
        {
            Assert.IsNotNull(random);
            _random = random;

            _animation = GetComponentInChildren<Animation>();
            Assert.IsNotNull(_animation);

            gameObject.SetActive(false);
        }

        public void Activate(Vector2 position)
        {
            gameObject.transform.position = position;
            gameObject.SetActive(true);

            AnimationState state = _animation["BuilderDrone"];
            Assert.IsNotNull(state);
            state.normalizedTime = _random.Value;
            _animation.Play();
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
            Deactivated?.Invoke(this, EventArgs.Empty);
        }
    }
}