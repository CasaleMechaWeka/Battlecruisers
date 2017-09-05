using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Offensive
{
    public class DeathstarLauncherController : SatelliteLauncherController
	{
		protected override Vector3 SpawnPositionAdjustment { get { return new Vector3(0, 0.015f, 0); } }
        public override TargetValue TargetValue { get { return TargetValue.High; } }
	}
}
