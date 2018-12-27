using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Utils;
using System;
using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators
{
    public class AngleCalculator : IAngleCalculator
	{
        private readonly IAngleHelper _angleHelper;
        protected readonly IFlightStats _projectileFlightStats;

		protected virtual bool LeadsTarget { get { return false; } }
		protected virtual bool MustFaceTarget { get { return false; } }

        public AngleCalculator(IAngleHelper angleHelper, IFlightStats projectileFlightStats)
        {
            Helper.AssertIsNotNull(angleHelper, projectileFlightStats);

            _angleHelper = angleHelper;
            _projectileFlightStats = projectileFlightStats;
        }

        public float FindDesiredAngle(Vector2 sourcePosition, Vector2 targetPosition, bool isSourceMirrored)
		{
            if (MustFaceTarget && !Helper.IsFacingTarget(targetPosition, sourcePosition, isSourceMirrored))
			{
                throw new ArgumentException("Source does not face target :(  source: " + sourcePosition + "  target: " + targetPosition + "  isSourceMirrored: " + isSourceMirrored);
            }

            return CalculateDesiredAngle(sourcePosition, targetPosition, isSourceMirrored);
        }

		/// <summary>
		/// Assumes shells are NOT affected by gravity
		/// </summary>
		protected virtual float CalculateDesiredAngle(Vector2 sourcePosition, Vector2 targetPosition, bool isSourceMirrored)
		{
            return _angleHelper.FindAngle(sourcePosition, targetPosition, isSourceMirrored);
		}
	}
}
