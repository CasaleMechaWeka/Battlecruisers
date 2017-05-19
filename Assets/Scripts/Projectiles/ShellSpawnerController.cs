using BattleCruisers.Projectiles;
using BattleCruisers.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Projectiles
{
	public class ShellSpawnerController : ProjectileSpawner
	{
		public void SpawnShell(float angleInDegrees, bool isSourceMirrored)
		{
			ShellController shell = Instantiate<ShellController>(_shellStats.ShellPrefab, transform.position, new Quaternion());
			Vector2 shellVelocity = FindShellVelocity(angleInDegrees, isSourceMirrored);
			float shellGravityScale = _shellStats.IgnoreGravity ? 0 : 1;
			shell.Initialise(_faction, _shellStats.Damage, shellVelocity, shellGravityScale);
		}

		private Vector2 FindShellVelocity(float angleInDegrees, bool isSourceMirrored)
		{
			float angleInRadians = angleInDegrees * Mathf.Deg2Rad;

			int xDirectionMultiplier = isSourceMirrored ? -1 : 1;

			float velocityX = _shellStats.VelocityInMPerS * Mathf.Cos(angleInRadians) * xDirectionMultiplier;
			float velocityY = _shellStats.VelocityInMPerS * Mathf.Sin(angleInRadians);

			Logging.Log(Tags.SHELL_SPAWNER, string.Format("angleInDegrees: {0}  isSourceMirrored: {1}  =>  velocityX: {2}  velocityY: {3}",
				angleInDegrees, isSourceMirrored, velocityX, velocityY));

			return new Vector2(velocityX, velocityY);
		}
	}
}
