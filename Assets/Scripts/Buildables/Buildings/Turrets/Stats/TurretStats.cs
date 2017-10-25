using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.Stats
{
    public class TurretStats : BasicTurretStats
	{
		public float accuracy;
		public float turretRotateSpeedInDegrees;

		public virtual bool IsInBurst { get { return false; } }

		public override void Initialise()
		{
            base.Initialise();

			Assert.IsTrue(accuracy >= 0 && accuracy <= 1);
			Assert.IsTrue(turretRotateSpeedInDegrees > 0);
		}
	}
}
