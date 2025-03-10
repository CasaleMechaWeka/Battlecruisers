namespace BattleCruisers.Buildables.Units.Aircraft
{
    public abstract class SatelliteController : AircraftController
    {
        public override void StartConstruction()
        {
            base.StartConstruction();

            // Satellites insta-complete
            OnBuildableCompleted();
        }
    }
}
