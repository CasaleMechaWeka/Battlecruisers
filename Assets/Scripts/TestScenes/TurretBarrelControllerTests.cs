using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.TestScenes
{
	public class TurretBarrelControllerTests : MonoBehaviour 
	{
		public GameObject target;

		public TurretBarrelController barrelBelowToLeft;
		public TurretBarrelController barrelBelowToRight;
		public TurretBarrelController barrelAboveToLeft;
		public TurretBarrelController barrelAboveToRight;

		public TurretBarrelController barrelBelowToLeftMirroed;
		public TurretBarrelController barrelBelowToRightMirroed;
		public TurretBarrelController barrelAboveToLeftMirroed;
		public TurretBarrelController barrelAboveToRightMirroed;

		void Start () 
		{
			Logging.Initialise();

			barrelBelowToLeft.Target = target;
			barrelBelowToRight.Target = target;
			barrelAboveToLeft.Target = target;
			barrelAboveToRight.Target = target;

			barrelBelowToLeftMirroed.Target = target;
			barrelBelowToRightMirroed.Target = target;
			barrelAboveToLeftMirroed.Target = target;
			barrelAboveToRightMirroed.Target = target;
		}
	}
}
