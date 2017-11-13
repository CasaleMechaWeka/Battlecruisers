using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators
{
    public class StaticAngleCalculator : AngleCalculator
	{
		private readonly float _desiredAngleInDegrees;

        public StaticAngleCalculator(IAngleHelper angleHelper, float desiredAngleInDegrees)
            : base(angleHelper)
		{ 
			_desiredAngleInDegrees = desiredAngleInDegrees;
		}

		protected override float CalculateDesiredAngle(Vector2 sourcePosition, Vector2 targetPosition, bool isSourceMirrored, float projectileVelocityInMPerS)
		{
			return _desiredAngleInDegrees;
		}
	}
}
