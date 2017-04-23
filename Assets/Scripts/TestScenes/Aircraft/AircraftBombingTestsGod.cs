using BattleCruisers.Units.Aircraft;
using BattleCruisers.Utils;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BattleCruisers.TestScenes.Aircraft
{
	public class AircraftBombingTestsGod : MonoBehaviour 
	{
		public BomberController bomberToLeft;
		public BomberController bomberToRight;

		public GameObject targetToLeft;
		public GameObject targetToRight;

		void Start() 
		{
			bomberToLeft.Target = targetToRight;
			bomberToRight.Target = targetToLeft;
		}
	}
}
