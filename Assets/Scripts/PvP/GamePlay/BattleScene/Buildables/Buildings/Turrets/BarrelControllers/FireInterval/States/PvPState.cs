using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelControllers.FireInterval.States
{
    public abstract class PvPState : IPvPState
    {
        protected IPvPState _otherState;
        protected IDurationProvider _durationProvider;

        public abstract bool ShouldFire { get; }

        // No constructor due to circular dependency :)
        public virtual void Initialise(IPvPState otherState, IDurationProvider durationProvider)
        {
            // Helper.AssertIsNotNull(otherState, durationProvider);

            _otherState = otherState;
            _durationProvider = durationProvider;
        }

        public abstract IPvPState OnFired();
        public abstract IPvPState ProcessTimeInterval(float timePassedInS);
    }
}
