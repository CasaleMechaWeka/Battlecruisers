namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval
{
    public interface IFireIntervalManager
	{
		bool ShouldFire();
        void OnFired();
        void Update(float deltaTime);
	}
}
