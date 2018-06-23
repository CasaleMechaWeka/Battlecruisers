using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.Stats
{
    public class TurretStats : BasicTurretStats, ITurretStats
	{
		public float accuracy;
		public float Accuracy { get { return accuracy; } }
		
        public float turretRotateSpeedInDegrees;
		public float TurretRotateSpeedInDegrees { get { return turretRotateSpeedInDegrees; } }

        public virtual bool IsInBurst { get { return false; } }

        public virtual int BurstSize { get { return DEFAULT_BURST_SIZE; } }

        private const int DEFAULT_BURST_SIZE = 1;

        public override void Initialise()
		{
            base.Initialise();

			Assert.IsTrue(accuracy >= 0 && accuracy <= 1);
			Assert.IsTrue(turretRotateSpeedInDegrees > 0);
		}
	}
}
