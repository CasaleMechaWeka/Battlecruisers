using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Targets.TargetDetectors
{
	public class CircleTargetDetector : TargetDetector
	{
		private CircleCollider2D _circleCollider;
        private float _radiusInM;

		public void Initialise(float radiusInM)
		{
            _radiusInM = radiusInM;

			_circleCollider = gameObject.GetComponent<CircleCollider2D>();
			Assert.IsNotNull(_circleCollider);
        }

        public override void StartDetecting()
        {
			_circleCollider.radius = _radiusInM;
        }
	}
}
