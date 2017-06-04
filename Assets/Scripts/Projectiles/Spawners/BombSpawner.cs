using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Projectiles.Spawners
{
	public class BombSpawner : BaseShellSpawner
	{
		public void SpawnShell(float currentXVelocityInMPers)
		{
			ProjectileController shell = Instantiate<ProjectileController>(_shellStats.ShellPrefab, transform.position, new Quaternion());
			Vector2 shellVelocity = new Vector2(currentXVelocityInMPers, 0);
			shell.Initialise(_shellStats, shellVelocity, _targetFilter);
		}
	}
}
