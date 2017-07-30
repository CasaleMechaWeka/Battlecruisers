namespace BattleCruisers.Buildables.Buildings.Factories
{
    public class DroneStation : Building
	{
		public int numOfDronesProvided;

		public override TargetValue TargetValue { get { return TargetValue.Medium; } }

		protected override void OnBuildableCompleted()
		{
			base.OnBuildableCompleted();

			_droneManager.NumOfDrones += numOfDronesProvided;
		}

		protected override void OnDestroyed()
		{
			base.OnDestroyed();

			_droneManager.NumOfDrones -= numOfDronesProvided;
		}
	}
}
