using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings
{
	public class BuildingWrapper : MonoBehaviour 
	{
		public Building Building { get; private set; }

		public void Awake()
		{
			Building = gameObject.GetComponentInChildren<Building>();
			Assert.IsNotNull(Building);
		}
	}
}
