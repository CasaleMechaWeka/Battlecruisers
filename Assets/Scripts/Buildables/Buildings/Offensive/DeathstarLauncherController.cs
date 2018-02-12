using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Offensive
{
    public class DeathstarLauncherController : SatelliteLauncherController
	{
		protected override Vector3 SpawnPositionAdjustment { get { return new Vector3(0, 0.015f, 0); } }
        public override TargetValue TargetValue { get { return TargetValue.High; } }

        protected override void OnStaticInitialised()
        {
            base.OnStaticInitialised();

            // Need satellite to be initialised to be able to access damage capabilities.
            satellitePrefab.Buildable.StaticInitialise();

            foreach (IDamageCapability damageCapability in satellitePrefab.Buildable.DamageCapabilities)
            {
                AddDamageStats(damageCapability);
            }
        }
	}
}
