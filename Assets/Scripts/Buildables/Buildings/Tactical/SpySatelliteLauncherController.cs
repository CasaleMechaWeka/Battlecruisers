using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Tactical
{
    public class SpySatelliteLauncherController : SatelliteLauncherController, IBuilding
    {
        // this class is needed for FogOfWarManager to work
        protected override Vector3 SpawnPositionAdjustment => new Vector3(0, 0.17f, 0);
        public override TargetValue TargetValue => TargetValue.Medium;
    }
}