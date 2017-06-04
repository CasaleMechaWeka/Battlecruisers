using BattleCruisers.Buildables;
using BattleCruisers.Projectiles;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Projectiles.Spawners
{
	public class ShellSpawner : BaseShellSpawner
	{
		public void SpawnShell(float angleInDegrees, bool isSourceMirrored)
		{
			ProjectileController shell = Instantiate<ProjectileController>(_shellStats.ShellPrefab, transform.position, new Quaternion());
			Vector2 shellVelocity = FindProjectileVelocity(angleInDegrees, isSourceMirrored, _shellStats.MaxVelocityInMPerS);
			shell.Initialise(_shellStats, shellVelocity, _targetFilter);
		}
	}
}
