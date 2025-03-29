using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelWrappers
{
    /// <summary>
    /// Turrets:  Mortar, artillery, broadsides
    /// </summary>
    public class PvPGravityAffectedBarrelWrapper : PvPBarrelWrapper
    {
        private const float X_MARGIN = 0.75f;

        [Header("True to use lower arc (artillery), false to use higher arc (mortar)")]
        public bool useLowerArc = true;

        protected override IAngleCalculator CreateAngleCalculator(IProjectileStats projectileStats)
        {
            return new GravityAffectedAngleCalculator(projectileStats, !useLowerArc);
        }

        protected override AccuracyAdjuster CreateAccuracyAdjuster(IAngleCalculator angleCalculator, IBarrelController barrel)
        {
            if (barrel.TurretStats.Accuracy >= Constants.MAX_ACCURACY)
            {
                return new AccuracyAdjuster((0, 0));
            }
            else
            {
                return new AccuracyAdjuster((X_MARGIN, 0f), angleCalculator, barrel.TurretStats);
            }
        }

        protected override AngleLimiter CreateAngleLimiter()
        {
            return new AngleLimiter(-20, 85);
        }
    }
}
