using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.UI.BattleScene.ProgressBars;
using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Offensive
{
    public class DeathstarLauncherController : SatelliteLauncherController
    {
        protected override Vector3 SpawnPositionAdjustment => new Vector3(0.003f, 0.21f, 0);
        public override TargetValue TargetValue => TargetValue.High;

        public override void StaticInitialise(GameObject parent, HealthBarController healthBar)
        {
            base.StaticInitialise(parent, healthBar);

            // Need satellite to be initialised to be able to access damage capabilities.
            satellitePrefab.StaticInitialise();

            foreach (IDamageCapability damageCapability in satellitePrefab.Buildable.DamageCapabilities)
            {
                AddDamageStats(damageCapability);
            }
        }
    }
}
