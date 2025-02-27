using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval.States;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelControllers.FireInterval.States
{
    public class PvPFiringOnceState : PvPState
    {
        public override bool ShouldFire => true;

        public override IState OnFired()
        {
            _durationProvider.MoveToNextDuration();
            return _otherState;
        }

        public override IState ProcessTimeInterval(float timePassedInS)
        {
            // Do nothing
            return this;
        }
    }
}