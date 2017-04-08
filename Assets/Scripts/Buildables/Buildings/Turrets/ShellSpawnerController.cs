using BattleCruisers.Units;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Buildings.Turrets
{
	public class ShellStats
	{
		public Rigidbody2D ShellPrefab { get; private set; }
		public float Damage { get; private set; }
		public bool IgnoreGravity { get; private set; }
		public float VelocityInMPerS { get; private set; }

		public ShellStats(Rigidbody2D shellPrefab, float damage, bool ignoreGravity, float velocityInMPerS)
		{
			ShellPrefab = shellPrefab;
			Damage = damage;
			IgnoreGravity = ignoreGravity;
			VelocityInMPerS = velocityInMPerS;
		}
	}

	public class ShellSpawnerController : MonoBehaviour 
	{
		private ShellStats _shellStats;

		public void Initialise(ShellStats shellStats)
		{
			_shellStats = shellStats;
		}

		public void SpawnShell(float angleInRadians, Direction fireDirection)
		{
			Rigidbody2D shell = Instantiate<Rigidbody2D>(_shellStats.ShellPrefab, transform.position, new Quaternion());
			if (_shellStats.IgnoreGravity)
			{
				shell.gravityScale = 0;
			}
			shell.GetComponent<IShellController>().Damage = _shellStats.Damage;
			shell.velocity = FindShellVelocity(angleInRadians, fireDirection);
		}

		private Vector2 FindShellVelocity(float angleInRadians, Direction fireDirection)
		{
			float xMultipler = fireDirection == Direction.Right ? 1 : -1;
			float velocityX = (float)(_shellStats.VelocityInMPerS * Math.Cos(angleInRadians)) * xMultipler;
			float velocityY = (float)(_shellStats.VelocityInMPerS * Math.Sin(angleInRadians));
			return new Vector2(velocityX, velocityY);
		}
	}
}
