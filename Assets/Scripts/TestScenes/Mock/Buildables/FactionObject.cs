using BattleCruisers.Buildables;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.TestScenes.Mock
{
	public class FactionObject : IFactionable
	{
		public bool IsDestroyed { get; set; }
		public float Health { get; set; }
		public Faction Faction { get; set; }
		public GameObject GameObject { get; set; }

		#pragma warning disable 67  // Unused event
		public event EventHandler Destroyed;
		public event EventHandler<HealthChangedEventArgs> HealthChanged;
		public event EventHandler FullyRepaired;
		#pragma warning restore 67  // Unused event

		public void TakeDamage(float damageAmount)
		{
			throw new NotImplementedException();
		}

		public void Repair(float repairAmount)
		{
			throw new NotImplementedException();
		}
	}
}
