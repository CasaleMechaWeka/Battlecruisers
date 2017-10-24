namespace BattleCruisers.Movement.Predictors
{
    public interface ITargetPositionPredictorFactory
	{
		ITargetPositionPredictor CreateLinearPredictor();
		ITargetPositionPredictor CreateMortarPredictor();
	}

	public class TargetPositionPredictorFactory : ITargetPositionPredictorFactory
	{
		public ITargetPositionPredictor CreateLinearPredictor()
		{
			return new LinearTargetPositionPredictor();
		}

		public ITargetPositionPredictor CreateMortarPredictor()
		{
			return new MortarTargetPositionPredictor();
		}
	}
}
