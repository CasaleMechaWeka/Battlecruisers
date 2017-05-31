using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Projectiles.Spawners
{
	public class BombSpawnerController : BaseShellSpawner
	{
		public void SpawnShell(float currentXVelocityInMPers)
		{
			ShellController shell = Instantiate<ShellController>(_shellStats.ShellPrefab, transform.position, new Quaternion());
			Vector2 shellVelocity = new Vector2(currentXVelocityInMPers, 0);
			shell.Initialise(_faction, _shellStats.Damage, shellVelocity, gravityScale: 1);
		}
	}
}
