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

		public event EventHandler Destroyed;
		public event EventHandler<HealthChangedEventArgs> HealthChanged;
		public event EventHandler FullyRepaired;

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
