using BattleCruisers.Buildables;
using BattleCruisers.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Projectiles.Spawners
{
	public class ShellStats
	{
		public ShellController ShellPrefab { get; private set; }
		public float Damage { get; private set; }
		public bool IgnoreGravity { get; private set; }
		public float VelocityInMPerS { get; private set; }

		public ShellStats(ShellController shellPrefab, float damage, bool ignoreGravity, float velocityInMPerS)
		{
			ShellPrefab = shellPrefab;
			Damage = damage;
			IgnoreGravity = ignoreGravity;
			VelocityInMPerS = velocityInMPerS;
		}
	}

	public abstract class ProjectileSpawner : MonoBehaviour
	{
		protected Faction _faction;
		protected ShellStats _shellStats;

		public void Initialise(Faction faction, ShellStats shellStats)
		{
			_faction = faction;
			_shellStats = shellStats;
		}
	}
}
