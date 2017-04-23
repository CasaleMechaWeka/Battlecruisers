using BattleCruisers.Buildables.Buildings.Turrets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Projectiles
{
	public class BomberStats : MonoBehaviour
	{
		public ShellController shellPrefab;
		public float damage;

		void Awake()
		{
			Assert.IsNotNull(shellPrefab);
			Assert.IsTrue(damage > 0);
		}
	}
}
