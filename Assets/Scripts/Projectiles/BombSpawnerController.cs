using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Turrets;

namespace BattleCruisers.Projectiles
{
	public class BombSpawnerController : MonoBehaviour
	{
		protected Faction _faction;
		protected ShellStats _shellStats;

		public void Initialise(Faction faction, ShellStats shellStats)
		{
			_faction = faction;
			_shellStats = shellStats;
		}

		public void SpawnShell(float currentXVelocityInMPers)
		{
			ShellController shell = Instantiate<ShellController>(_shellStats.ShellPrefab, transform.position, new Quaternion());
			Vector2 shellVelocity = new Vector2(currentXVelocityInMPers, 0);
			shell.Initialise(_faction, _shellStats.Damage, shellVelocity, gravityScale: 1);
		}
	}
}
