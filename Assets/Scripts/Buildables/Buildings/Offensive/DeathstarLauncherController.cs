using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;
using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Offensive
{
    public class DeathstarLauncherController : SatelliteLauncherController
	{
		protected override Vector3 SpawnPositionAdjustment { get { return new Vector3(0.003f, 0.145f, 0); } }
        protected override PrioritisedSoundKey ConstructionCompletedSoundKey { get { return PrioritisedSoundKeys.Completed.Ultra; } }
        public override TargetValue TargetValue { get { return TargetValue.High; } }

        protected override void OnStaticInitialised()
        {
            base.OnStaticInitialised();

            // Need satellite to be initialised to be able to access damage capabilities.
            satellitePrefab.Initialise();

            foreach (IDamageCapability damageCapability in satellitePrefab.Buildable.DamageCapabilities)
            {
                AddDamageStats(damageCapability);
            }
        }
	}
}
