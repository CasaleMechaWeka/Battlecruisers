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
        private ILaserCollisionDetector _collisionDetector;
		private float _damagePerS;
        private ITarget _parent;

		public LayerMask unitsLayerMask, shieldsLayerMask;

		private const int NUM_OF_COLLIDERS_TO_RAYCAST = 25;

		void Awake() 
		{
            LineRenderer lineRenderer = GetComponent<LineRenderer>();
            _laserRenderer = new LaserRenderer(lineRenderer);
		}

        public void Initialise(ITargetFilter targetFilter, float damagePerS, ITarget parent)
		{
            Helper.AssertIsNotNull(targetFilter, parent);
            Assert.IsTrue(damagePerS > 0);

			_damagePerS = damagePerS;
            _parent = parent;

            ContactFilter2D contactFilter = new ContactFilter2D()
            {
                useLayerMask = true,
                layerMask = unitsLayerMask.value | shieldsLayerMask.value,
                useTriggers = true
            };
            _collisionDetector = new LaserCollisionDetector(contactFilter, targetFilter);
		}

		public void FireLaser(float angleInDegrees, bool isSourceMirrored)
		{
            ILaserCollision collision = _collisionDetector.FindCollision(transform.position, angleInDegrees, isSourceMirrored);
			
			if (collision != null)
			{
                _laserRenderer.ShowLaser(transform.position, collision.CollisionPoint);

				float damage = Time.deltaTime * _damagePerS;
                collision.Target.TakeDamage(damage, _parent);
			}
		}

		public void StopLaser()
		{
            _laserRenderer.HideLaser();
		}
	}
}
