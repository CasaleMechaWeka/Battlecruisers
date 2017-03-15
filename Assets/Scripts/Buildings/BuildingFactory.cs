using BattleCruisers.Buildings.Turrets;
using BattleCruisers.Cruisers;
using BattleCruisers.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Buildings
{
	public class BuildingFactory : MonoBehaviour 
	{
		public Cruiser cruiser1;
		public Cruiser cruiser2;
		public UIManager uiManager;

		public Building CreateBuilding(Building buildingPrefab, Cruiser parentCruiser)
		{
			Building building = Instantiate<Building>(buildingPrefab);
			building.UIManager = uiManager;
			building.ParentCruiser = parentCruiser;

			Turret turret = building as Turret;
			if (turret != null)
			{
				// FELIX  Don't hardcode all turret stats :P
				turret.TurretStats = new TurretStats(0.3f, 0.5f, 250f, 30f, ignoreGravity: false);
				turret.Target = GetEnemyCruiser(parentCruiser).gameObject;
			}

			return building;
		}

		private Cruiser GetEnemyCruiser(Cruiser parentCruiser)
		{
			if (cruiser1 == parentCruiser)
			{
				return cruiser2;
			}
			else if (cruiser2 == parentCruiser)
			{
				return cruiser1;
			}
			else
			{
				throw new InvalidProgramException();
			}
		}
	}
}
