using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Targets.TargetFinders.Filters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Projectiles.Spawners
{
	// FELIX  Separate racast logic?  Make this a dumber class that just draws a laser between 2 points?
	public class LaserEmitter : MonoBehaviour 
	{
		private LineRenderer _lineRenderer;
		private ContactFilter2D _contactFilter;
		private ITargetFilter _targetFilter;
		private float _damagePerS;

		public LayerMask unitsLayerMask;

		void Awake() 
		{
			_lineRenderer = GetComponent<LineRenderer>();
			Assert.IsNotNull(_lineRenderer);
			_lineRenderer.positionCount = 2;

			_contactFilter = new ContactFilter2D() 
			{
				useLayerMask = true,
				layerMask = unitsLayerMask
			};
		}

		public void Initialise(ITargetFilter targetFilter, float damagePerS)
		{
			_targetFilter = targetFilter;
			_damagePerS = damagePerS;
		}

		public void FireLaser(float angleInDegrees, bool isSourceMirrored)
		{
			// FELIX  Test when isSourceMirrored is true
			float xComponent = Mathf.Cos(Mathf.Deg2Rad * angleInDegrees);
			float yComponent = Mathf.Sin(Mathf.Deg2Rad * angleInDegrees);
			Vector2 laserDirection = new Vector2(xComponent, yComponent);

			// FELIX  Magic number
			RaycastHit2D[] results = new RaycastHit2D[25];
			int numOfResults = Physics2D.Raycast(transform.position, laserDirection, _contactFilter, results);
			
			ITarget target = GetTarget(results, numOfResults);
			
			if (target != null)
			{
				_lineRenderer.SetPosition(0, transform.position);
				_lineRenderer.SetPosition(1, target.Position);

				float damage = Time.deltaTime * _damagePerS;
				target.TakeDamage(damage);
			}
		}

		// FELIX  Also return RaycastHit2D, so can draw laser to contact point :)
		private ITarget GetTarget(RaycastHit2D[] results, int numOfResults)
		{
			for (int i = 0; i < numOfResults; i++)
			{
				RaycastHit2D result = results[i];
				Assert.IsNotNull(result.collider);
				
				ITarget target = result.collider.gameObject.GetComponent<ITarget>();
				
				if (target != null && _targetFilter.IsMatch(target))
				{
					return target;
				}
			}

			return null;
		}
	}
}
