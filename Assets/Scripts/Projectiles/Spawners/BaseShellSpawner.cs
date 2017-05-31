using BattleCruisers.Buildables;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Projectiles.Spawners
{
	public abstract class BaseShellSpawner : ProjectileSpawner
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
