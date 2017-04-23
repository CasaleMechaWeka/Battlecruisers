using BattleCruisers.Buildables.Buildings.Tactical;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.TestScenes
{
	public class ShieldTestsGod : MonoBehaviour 
	{
		public ShieldController shield;
		public TurretBarrelController turret;

		void Start () 
		{
			turret.Target = shield.gameObject;
		}
	}
}
