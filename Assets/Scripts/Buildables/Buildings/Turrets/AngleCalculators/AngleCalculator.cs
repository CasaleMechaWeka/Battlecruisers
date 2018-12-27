using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators
{
    public class AngleCalculator : IAngleCalculator
	{
        private readonly IAngleHelper _angleHelper;
        protected readonly IProjectileFlightStats _projectileFlightStats;

        public AngleCalculator(IAngleHelper angleHelper, IProjectileFlightStats projectileFlightStats)
        {
            Helper.AssertIsNotNull(angleHelper, projectileFlightStats);

            _angleHelper = angleHelper;
            _projectileFlightStats = projectileFlightStats;
        }

        public virtual float FindDesiredAngle(Vector2 sourcePosition, Vector2 targetPosition, bool isSourceMirrored)
		{
            return _angleHelper.FindAngle(sourcePosition, targetPosition, isSourceMirrored);
        }
	}
}
