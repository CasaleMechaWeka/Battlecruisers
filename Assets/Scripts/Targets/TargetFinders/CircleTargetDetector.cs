using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Targets.TargetFinders
{
	public class CircleTargetDetector : TargetDetector
	{
		private CircleCollider2D _circleCollider;
        private float _radiusInM;

        // FELIX  Do not allow default parameter (I think :P )
		public void Initialise(float radiusInM = -1)
		{
            _radiusInM = radiusInM;

			_circleCollider = gameObject.GetComponent<CircleCollider2D>();
			Assert.IsNotNull(_circleCollider);
        }

        public override void StartDetecting()
        {
			if (_radiusInM != -1)
			{
				_circleCollider.radius = _radiusInM;
			}
        }
	}
}
