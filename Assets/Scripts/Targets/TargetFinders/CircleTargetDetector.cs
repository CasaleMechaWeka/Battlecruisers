using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Targets.TargetFinders
{
	public class CircleTargetDetector : TargetDetector
	{
		private CircleCollider2D _circleCollider;

		public void Initialise(float radiusInM = -1)
		{
			_circleCollider = gameObject.GetComponent<CircleCollider2D>();
			Assert.IsNotNull(_circleCollider);

			if (radiusInM != -1)
			{
				_circleCollider.radius = radiusInM;
			}
		}
	}
}
