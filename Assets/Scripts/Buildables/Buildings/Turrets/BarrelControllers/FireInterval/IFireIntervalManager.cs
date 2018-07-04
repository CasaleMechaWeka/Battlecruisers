namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval
{
    public interface IFireIntervalManager
	{
		bool ShouldFire { get; }

        void OnFired();
        void ProcessTimeInterval(float deltaTime);
	}
}
