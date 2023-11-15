namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelControllers.FireInterval
{
    public interface IPvPDurationProvider
    {
        float DurationInS { get; }
        void MoveToNextDuration();
    }
}
