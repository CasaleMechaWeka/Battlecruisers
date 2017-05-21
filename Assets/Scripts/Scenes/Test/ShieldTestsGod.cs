using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Tactical;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Scenes.Test
{
	public class ShieldTestsGod : MonoBehaviour 
	{
		public ShieldController shield;
		public TurretBarrelController turret;

		void Start () 
		{
			shield.Initialise(Faction.Reds);

			turret.Initialise(Faction.Blues);
			turret.Target = shield;
		}
	}
}
