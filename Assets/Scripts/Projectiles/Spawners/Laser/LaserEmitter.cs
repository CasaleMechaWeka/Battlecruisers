using BattleCruisers.Buildables;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Projectiles.Spawners.Laser
{
    public class LaserEmitter : MonoBehaviour 
	{
        private ILaserRenderer _laserRenderer;
		private ContactFilter2D _contactFilter;
		private ITargetFilter _targetFilter;
		private float _damagePerS;
        private ITarget _parent;

		public LayerMask unitsLayerMask, shieldsLayerMask;

		private const int NUM_OF_COLLIDERS_TO_RAYCAST = 25;

		void Awake() 
		{
            LineRenderer lineRenderer = GetComponent<LineRenderer>();
            _laserRenderer = new LaserRenderer(lineRenderer);

			_contactFilter = new ContactFilter2D() 
			{
				useLayerMask = true,
				layerMask = unitsLayerMask.value | shieldsLayerMask.value,
                useTriggers = true
			};
		}

        public void Initialise(ITargetFilter targetFilter, float damagePerS, ITarget parent)
		{
            Helper.AssertIsNotNull(targetFilter, parent);
            Assert.IsTrue(damagePerS > 0);

			_targetFilter = targetFilter;
			_damagePerS = damagePerS;
            _parent = parent;
		}

		public void FireLaser(float angleInDegrees, bool isSourceMirrored)
		{
			Vector2 laserDirection = FindLaserDirection(angleInDegrees, isSourceMirrored);

			RaycastHit2D[] results = new RaycastHit2D[NUM_OF_COLLIDERS_TO_RAYCAST];
			int numOfResults = Physics2D.Raycast(transform.position, laserDirection, _contactFilter, results);
			
            ILaserCollision collision = GetTarget(results, numOfResults);
			
			if (collision != null)
			{
                _laserRenderer.ShowLaser(transform.position, collision.CollisionPoint);

				float damage = Time.deltaTime * _damagePerS;
                collision.Target.TakeDamage(damage, _parent);
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
            _laserRenderer.HideLaser();
		}
	}
}
