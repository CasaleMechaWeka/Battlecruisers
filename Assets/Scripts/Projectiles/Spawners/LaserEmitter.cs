using BattleCruisers.Buildables;
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
		private Vector2 _laserDirection;
		private ContactFilter2D _contactFilter;
		private bool _isLaserActive;
		private ITargetFilter _targetFilter;

		public LayerMask layerMask;

		void Awake() 
		{
			_lineRenderer = GetComponent<LineRenderer>();
			Assert.IsNotNull(_lineRenderer);
			_lineRenderer.positionCount = 2;

			_isLaserActive = false;

			_contactFilter = new ContactFilter2D() 
			{
				useLayerMask = true,
				layerMask = layerMask
			};
		}

		public void Initialise(ITargetFilter targetFilter, bool isMirrored)
		{
			_targetFilter = targetFilter;
			_laserDirection = isMirrored ? Vector2.left : Vector2.right;
		}

		public void StartLaser()
		{
			_isLaserActive = true;
		}

		public void StopLaser()
		{
			_isLaserActive = false;
		}

		void Update()
		{
			if (_isLaserActive)
			{
				// FELIX  Magic number
				RaycastHit2D[] results = new RaycastHit2D[25];
				int numOfResults = Physics2D.Raycast(transform.position, _laserDirection, _contactFilter, results);

				ITarget target = GetTarget(results, numOfResults);

				if (target != null)
				{
					_lineRenderer.SetPosition(0, transform.position);
					_lineRenderer.SetPosition(1, target.Position);
				}
			}
		}

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
