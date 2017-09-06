using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Tactical
{
	public class SpySatelliteLauncherController : SatelliteLauncherController, ISpySatelliteLauncher
	{
		protected override Vector3 SpawnPositionAdjustment { get { return new Vector3(0, 0.063f, 0); } }
        public override TargetValue TargetValue { get { return TargetValue.Medium; } }
	}
}
