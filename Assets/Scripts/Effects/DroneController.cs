using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Effects
{
    // FELIX  Make poolable :)
    public class DroneController : MonoBehaviour
    {
        private IRandomGenerator _random;
        private Animation _animation;

        public void Initialise(IRandomGenerator random)
        {
            Assert.IsNotNull(random);
            _random = random;

            _animation = GetComponentInChildren<Animation>();
            Assert.IsNotNull(_animation);
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
        }
    }
}