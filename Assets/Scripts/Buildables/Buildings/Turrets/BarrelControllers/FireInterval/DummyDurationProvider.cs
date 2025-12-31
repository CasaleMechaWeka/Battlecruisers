namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval
{
    public class DummyDurationProvider : IDurationProvider
	{
        public float DurationInS { get; }

        public DummyDurationProvider(float durationInS)
        {
            DurationInS = durationInS;
        }

        public void MoveToNextDuration()
        {
            // Empty
        }
    }
}
