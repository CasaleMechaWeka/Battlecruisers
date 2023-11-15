namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelControllers.FireInterval.States
{
    public class PvPFiringOnceState : PvPState
    {
        public override bool ShouldFire => true;

        public override IPvPState OnFired()
        {
            _durationProvider.MoveToNextDuration();
            return _otherState;
        }

        public override IPvPState ProcessTimeInterval(float timePassedInS)
        {
            // Do nothing
            return this;
        }
    }
}