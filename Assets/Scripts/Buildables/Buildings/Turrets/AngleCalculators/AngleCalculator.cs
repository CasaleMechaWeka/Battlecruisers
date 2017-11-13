using System;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators
{
    public class AngleCalculator : IAngleCalculator
	{
        private readonly IAngleHelper _angleHelper;

		protected virtual bool LeadsTarget { get { return false; } }
		protected virtual bool MustFaceTarget { get { return false; } }

        public AngleCalculator(IAngleHelper angleHelper)
        {
            Assert.IsNotNull(angleHelper);
            _angleHelper = angleHelper;
        }

        public float FindDesiredAngle(Vector2 sourcePosition, Vector2 targetPosition, bool isSourceMirrored, float projectileVelocityInMPerS)
		{
            if (MustFaceTarget && !Helper.IsFacingTarget(targetPosition, sourcePosition, isSourceMirrored))
			{
                throw new ArgumentException("Source does not face target :(  source: " + sourcePosition + "  target: " + targetPosition + "  isSourceMirrored: " + isSourceMirrored);
            }

            return CalculateDesiredAngle(sourcePosition, targetPosition, isSourceMirrored, projectileVelocityInMPerS);
        }

		/// <summary>
		/// Assumes shells are NOT affected by gravity
		/// </summary>
		protected virtual float CalculateDesiredAngle(Vector2 sourcePosition, Vector2 targetPosition, bool isSourceMirrored, float projectileVelocityInMPerS)
		{
            return _angleHelper.FindAngle(sourcePosition, targetPosition, isSourceMirrored);
		}
	}
}
