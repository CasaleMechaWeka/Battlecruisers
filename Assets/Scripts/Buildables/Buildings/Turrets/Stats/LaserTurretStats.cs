using BattleCruisers.Projectiles;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Utils;
using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets
{
	public class LaserTurretStats : TurretStats
	{
		public float damagePerS;
		public float laserDurationInS;

		public override float DamagePerS { get { return damagePerS; } }
	}
}
