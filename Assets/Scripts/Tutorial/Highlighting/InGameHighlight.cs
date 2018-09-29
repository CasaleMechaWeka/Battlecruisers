using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Highlighting
{
    public class InGameHighlight : ScalableCircle, IHighlight
    {
        private Animator _animator;

        public void Initialise(float radiusInM, Vector2 position, bool usePulsingAnimation)
        {
            base.Initialise(radiusInM);

            transform.position = position;

            _animator = GetComponentInChildren<Animator>();
            Assert.IsNotNull(_animator);
            _animator.enabled = usePulsingAnimation;
        }

		public void Destroy()
		{
            Destroy(gameObject);
		}
    }
}
