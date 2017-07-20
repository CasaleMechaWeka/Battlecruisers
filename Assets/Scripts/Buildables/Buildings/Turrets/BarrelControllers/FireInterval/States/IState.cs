namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval.States
{
    public interface IState
    {
        bool ShouldFire { get; }

        IState ProcessTimeInterval(float timePassedInS);
    }
}
