using BattleCruisers.Buildables;
using BattleCruisers.Targets.TargetFinders.Filters;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Projectiles.Spawners
{
    public class LaserEmitter : MonoBehaviour 
	{
		private LineRenderer _lineRenderer;
		private ContactFilter2D _contactFilter;
		private ITargetFilter _targetFilter;
		private float _damagePerS;

		public LayerMask unitsLayerMask, shieldsLayerMask;

		private const int NUM_OF_COLLIDERS_TO_RAYCAST = 25;

		void Awake() 
		{
			_lineRenderer = GetComponent<LineRenderer>();
			Assert.IsNotNull(_lineRenderer);
			_lineRenderer.positionCount = 2;

			_contactFilter = new ContactFilter2D() 
			{
				useLayerMask = true,
				layerMask = unitsLayerMask.value | shieldsLayerMask.value,
                useTriggers = true
			};
		}

		public void Initialise(ITargetFilter targetFilter, float damagePerS)
		{
			_targetFilter = targetFilter;
			_damagePerS = damagePerS;
		}

		public void FireLaser(float angleInDegrees, bool isSourceMirrored)
		{
			Vector2 laserDirection = FindLaserDirection(angleInDegrees, isSourceMirrored);

			RaycastHit2D[] results = new RaycastHit2D[NUM_OF_COLLIDERS_TO_RAYCAST];
			int numOfResults = Physics2D.Raycast(transform.position, laserDirection, _contactFilter, results);
			
            ILaserCollision collision = GetTarget(results, numOfResults);
			
			if (collision != null)
			{
				_lineRenderer.enabled = true;
				_lineRenderer.SetPosition(0, transform.position);
                _lineRenderer.SetPosition(1, collision.CollisionPoint);

				float damage = Time.deltaTime * _damagePerS;
                collision.Target.TakeDamage(damage);
			}
		}

		static Vector2 FindLaserDirection(float angleInDegrees, bool isSourceMirrored)
		{
			float directionMultiplier = isSourceMirrored ? -1 : 1;
			float xComponent = Mathf.Cos(Mathf.Deg2Rad * angleInDegrees) * directionMultiplier;
			float yComponent = Mathf.Sin(Mathf.Deg2Rad * angleInDegrees);
			return new Vector2(xComponent, yComponent);
		}

        private ILaserCollision GetTarget(RaycastHit2D[] results, int numOfResults)
        {
            for (int i = 0; i < numOfResults; i++)
            {
                RaycastHit2D result = results[i];
                Assert.IsNotNull(result.collider);

                ITarget target = result.collider.gameObject.GetComponent<ITarget>();

                if (target != null && _targetFilter.IsMatch(target))
                {
                    return new LaserCollision(target, result.point);
                }
            }

            return null;
        }

		public void StopLaser()
		{
			_lineRenderer.enabled = false;
		}
	}
}
