namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelControllers.FireInterval.States
{
    public interface IPvPState
    {
        bool ShouldFire { get; }

        IPvPState ProcessTimeInterval(float timePassedInS);
        IPvPState OnFired();
    }
}
