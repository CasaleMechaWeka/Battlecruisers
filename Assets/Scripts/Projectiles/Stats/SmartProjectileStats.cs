using BattleCruisers.Buildables;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine.Assertions;

namespace BattleCruisers.Projectiles.Stats
{
    public class SmartProjectileStats : ProjectileStats
    {
        public float detectionRangeM;
        public float DetectionRangeM => detectionRangeM;

        public List<TargetType> attackCapabilities;
        public ReadOnlyCollection<TargetType> AttackCapabilities { get; private set; }

        protected override void OnAwake()
        {
            Assert.IsTrue(detectionRangeM > 0);
            Assert.IsTrue(attackCapabilities.Count > 0);

            AttackCapabilities = attackCapabilities.AsReadOnly();
        }

        public override void ApplyVariantStats(StatVariant statVariant)
        {
            if (!isAppliedVariant)
            {
                base.ApplyVariantStats(statVariant);
                detectionRangeM += statVariant.detection_range;
            }
        }
    }
}
