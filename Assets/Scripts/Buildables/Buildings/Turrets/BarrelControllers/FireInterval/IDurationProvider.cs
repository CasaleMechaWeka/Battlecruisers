namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval
{
    public interface IDurationProvider
	{
		float DurationInS { get; }
        void MoveToNextDuration();
	}
}
