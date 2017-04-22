using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Units
{
	public class BomberController : Unit
	{
		private IList<Vector2> _patrolPoints;
		private Vector2 _targetPatrolPoint;

		public void Initialise(IList<Vector2> patrolPoints)
		{
			Assert.IsTrue(patrolPoints.Count >= 2);

			_patrolPoints = patrolPoints;
		}

		protected override void OnUpdate()
		{
			base.OnUpdate();


		}

		// FELIX TEMP
		public void TempStartPatrolling()
		{
			StartPatrolling();
		}

		private void StartPatrolling()
		{

		}

		private Vector2 FindNearestPatrolPoint()
		{
			NEXT

			return new Vector2();
		}

		private Vector2 FindNextPatrolPoint()
		{
			return new Vector2();
		}
	}
}
