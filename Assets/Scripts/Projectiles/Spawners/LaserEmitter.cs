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

		void Awake() 
		{
			_lineRenderer = GetComponent<LineRenderer>();
			Assert.IsNotNull(_lineRenderer);
			_lineRenderer.positionCount = 2;
		}

		public void Initialise(bool isMirrored)
		{
			_laserDirection = isMirrored ? Vector2.left : Vector2.right;
		}

		public void StartLaser()
		{
			RaycastHit2D result = Physics2D.Raycast(transform.position, _laserDirection);
			if (result.collider != null)
			{
				_lineRenderer.SetPosition(0, transform.position);
				_lineRenderer.SetPosition(1, result.point);
			}
		}

		public void StopLaser()
		{

		}
	}
}
