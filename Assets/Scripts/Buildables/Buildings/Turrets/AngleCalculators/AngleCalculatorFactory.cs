using BattleCruisers.Movement.Predictors;
using System;

namespace BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators
{
	public interface IAngleCalculatorFactory
	{
		IAngleCalculator CreateAngleCalcultor(float projectileVelocityInMPerS, bool isSourceMirrored, ITargetPositionPredictorFactory targetPositionPredictorFactory);
		IAngleCalculator CreateArtilleryAngleCalcultor(float projectileVelocityInMPerS, bool isSourceMirrored, ITargetPositionPredictorFactory targetPositionPredictorFactory);
		IAngleCalculator CreateMortarAngleCalcultor(float projectileVelocityInMPerS, bool isSourceMirrored, ITargetPositionPredictorFactory targetPositionPredictorFactory);
		IAngleCalculator CreateLeadingAngleCalcultor(float projectileVelocityInMPerS, bool isSourceMirrored, ITargetPositionPredictorFactory targetPositionPredictorFactory);
	}

	public class AngleCalculatorFactory
	{
		public IAngleCalculator CreateAngleCalcultor(float projectileVelocityInMPerS, bool isSourceMirrored, ITargetPositionPredictorFactory targetPositionPredictorFactory)
		{
			return new AngleCalculator(projectileVelocityInMPerS, isSourceMirrored, targetPositionPredictorFactory);
		}

		public IAngleCalculator CreateArtilleryAngleCalcultor(float projectileVelocityInMPerS, bool isSourceMirrored, ITargetPositionPredictorFactory targetPositionPredictorFactory)
		{
			return new ArtilleryAngleCalculator(projectileVelocityInMPerS, isSourceMirrored, targetPositionPredictorFactory);
		}

		public IAngleCalculator CreateMortarAngleCalcultor(float projectileVelocityInMPerS, bool isSourceMirrored, ITargetPositionPredictorFactory targetPositionPredictorFactory)
		{
			return new MortarAngleCalculator(projectileVelocityInMPerS, isSourceMirrored, targetPositionPredictorFactory);
		}

		public IAngleCalculator CreateLeadingAngleCalcultor(float projectileVelocityInMPerS, bool isSourceMirrored, ITargetPositionPredictorFactory targetPositionPredictorFactory)
		{
			return new LeadingAngleCalculator(projectileVelocityInMPerS, isSourceMirrored, targetPositionPredictorFactory);
		}
	}
}

