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
	public class ShellSpawnerController : BaseShellSpawner
	{
		public void SpawnShell(float angleInDegrees, bool isSourceMirrored)
		{
			ShellController shell = Instantiate<ShellController>(_shellStats.ShellPrefab, transform.position, new Quaternion());
			Vector2 shellVelocity = FindProjectileVelocity(angleInDegrees, isSourceMirrored, _shellStats.VelocityInMPerS);
			float shellGravityScale = _shellStats.IgnoreGravity ? 0 : 1;
			shell.Initialise(_faction, _shellStats.Damage, shellVelocity, shellGravityScale);
		}
	}
}
