namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval
{
    public class DummyDurationProvider : IFireIntervalProvider
	{
        public float NextFireIntervalInS { get; private set; }

        public DummyDurationProvider(float durationInS)
        {
            NextFireIntervalInS = durationInS;
        }
    }
}
