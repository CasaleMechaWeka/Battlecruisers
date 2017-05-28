using BattleCruisers.Movement.Predictors;
using System;

namespace BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators
{
	public interface IAngleCalculatorFactory
	{
		IAngleCalculator CreateAngleCalcultor(ITargetPositionPredictorFactory targetPositionPredictorFactory);
		IAngleCalculator CreateArtilleryAngleCalcultor(ITargetPositionPredictorFactory targetPositionPredictorFactory);
		IAngleCalculator CreateMortarAngleCalcultor(ITargetPositionPredictorFactory targetPositionPredictorFactory);
		IAngleCalculator CreateLeadingAngleCalcultor(ITargetPositionPredictorFactory targetPositionPredictorFactory);
	}

	public class AngleCalculatorFactory : IAngleCalculatorFactory
	{
		public IAngleCalculator CreateAngleCalcultor(ITargetPositionPredictorFactory targetPositionPredictorFactory)
		{
			return new AngleCalculator(targetPositionPredictorFactory);
		}

		public IAngleCalculator CreateArtilleryAngleCalcultor(ITargetPositionPredictorFactory targetPositionPredictorFactory)
		{
			return new ArtilleryAngleCalculator(targetPositionPredictorFactory);
		}

		public IAngleCalculator CreateMortarAngleCalcultor(ITargetPositionPredictorFactory targetPositionPredictorFactory)
		{
			return new MortarAngleCalculator(targetPositionPredictorFactory);
		}

		public IAngleCalculator CreateLeadingAngleCalcultor(ITargetPositionPredictorFactory targetPositionPredictorFactory)
		{
			return new LeadingAngleCalculator(targetPositionPredictorFactory);
		}
	}
}

