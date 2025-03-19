using BattleCruisers.Buildables.Buildings.Turrets.AccuracyAdjusters;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.AngleLimiters;
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
            if (useLowerArc)
            {
                return _factoryProvider.Turrets.AngleCalculatorFactory.CreateArtilleryAngleCalculator(projectileStats);
            }
            else
            {
                return _factoryProvider.Turrets.AngleCalculatorFactory.CreateMortarAngleCalculator(projectileStats);
            }
        }

        protected override IAccuracyAdjuster CreateAccuracyAdjuster(IAngleCalculator angleCalculator, IBarrelController barrel)
        {
            if (barrel.TurretStats.Accuracy >= Constants.MAX_ACCURACY)
            {
                return new DummyAccuracyAdjuster();
            }
            else
            {
                return new AccuracyAdjuster((X_MARGIN, 0f), angleCalculator, RandomGenerator.Instance, barrel.TurretStats);
            }
        }

        protected override IAngleLimiter CreateAngleLimiter()
        {
            return new AngleLimiter(-20, 85);
        }
    }
}
