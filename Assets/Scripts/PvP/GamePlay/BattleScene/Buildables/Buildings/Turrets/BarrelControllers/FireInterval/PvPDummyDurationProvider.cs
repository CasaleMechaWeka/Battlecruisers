namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelControllers.FireInterval
{
    public class PvPDummyDurationProvider : IPvPDurationProvider
    {
        public float DurationInS { get; }

        public PvPDummyDurationProvider(float durationInS)
        {
            DurationInS = durationInS;
        }

        public void MoveToNextDuration()
        {
            // Empty
        }
    }
}
