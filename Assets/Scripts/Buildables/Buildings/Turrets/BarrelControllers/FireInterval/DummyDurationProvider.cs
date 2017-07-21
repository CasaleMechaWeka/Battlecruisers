namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval
{
    public class DummyDurationProvider : IDurationProvider
	{
        public float NextDurationInS { get; private set; }

        public DummyDurationProvider(float durationInS)
        {
            NextDurationInS = durationInS;
        }
    }
}
