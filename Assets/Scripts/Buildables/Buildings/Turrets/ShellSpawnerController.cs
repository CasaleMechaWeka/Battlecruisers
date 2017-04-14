using BattleCruisers.Buildables.Units;
using BattleCruisers.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Turrets
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

		public void SpawnShell(float angleInDegrees, bool isSourceMirrored)
		{
			Rigidbody2D shell = Instantiate<Rigidbody2D>(_shellStats.ShellPrefab, transform.position, new Quaternion());
			if (_shellStats.IgnoreGravity)
			{
				shell.gravityScale = 0;
			}
			shell.GetComponent<IShellController>().Damage = _shellStats.Damage;
			shell.velocity = FindShellVelocity(angleInDegrees, isSourceMirrored);
		}

		private Vector2 FindShellVelocity(float angleInDegrees, bool isSourceMirrored)
		{
			float angleInRadians = angleInDegrees * Mathf.Deg2Rad;

			int xDirectionMultiplier = isSourceMirrored ? -1 : 1;

			float velocityX = _shellStats.VelocityInMPerS * Mathf.Cos(angleInRadians) * xDirectionMultiplier;
			float velocityY = _shellStats.VelocityInMPerS * Mathf.Sin(angleInRadians);

			Logging.Log(Tags.SHELL_SPAWNER, $"angleInDegrees: {angleInDegrees}  isSourceMirrored: {isSourceMirrored}  =>  velocityX: {velocityX}  velocityY: {velocityY}");

			return new Vector2(velocityX, velocityY);
		}
	}
}
