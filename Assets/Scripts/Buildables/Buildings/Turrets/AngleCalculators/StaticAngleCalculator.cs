using BattleCruisers.Projectiles.Stats;
using NSubstitute;
using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators
{
    public class StaticAngleCalculator : AngleCalculator
	{
		private readonly float _desiredAngleInDegrees;

        public StaticAngleCalculator(IAngleHelper angleHelper, float desiredAngleInDegrees)
            : base(angleHelper, Substitute.For<IProjectileFlightStats>())
		{ 
			_desiredAngleInDegrees = desiredAngleInDegrees;
		}

		public override float FindDesiredAngle(Vector2 sourcePosition, Vector2 targetPosition, bool isSourceMirrored)
		{
			return _desiredAngleInDegrees;
		}
	}
}
