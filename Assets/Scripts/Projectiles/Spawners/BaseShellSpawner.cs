using BattleCruisers.Buildables;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Projectiles.Spawners
{
	public abstract class BaseShellSpawner : ProjectileSpawner
	{
		protected ShellStats _shellStats;
		protected ITargetFilter _targetFilter;

		public void Initialise(ShellStats shellStats, ITargetFilter targetFilter)
		{
			_shellStats = shellStats;
			_targetFilter = targetFilter;
		}
	}
}
