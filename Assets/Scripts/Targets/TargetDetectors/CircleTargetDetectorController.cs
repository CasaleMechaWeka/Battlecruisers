using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Targets.TargetDetectors
{
	public class CircleTargetDetectorController : TargetDetectorController
	{
		private CircleCollider2D _circleCollider;
        private float _radiusInM;

		public void Initialise(float radiusInM)
		{
            _radiusInM = radiusInM;

			_circleCollider = gameObject.GetComponent<CircleCollider2D>();
			Assert.IsNotNull(_circleCollider);
            _circleCollider.enabled = false;
        }

        public override void StartDetecting()
        {
			_circleCollider.radius = _radiusInM;
            _circleCollider.enabled = true;
        }
	}
}
