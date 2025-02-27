using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval.States;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelControllers.FireInterval.States
{
    public abstract class PvPState : IState
    {
        protected IState _otherState;
        protected IDurationProvider _durationProvider;

        public abstract bool ShouldFire { get; }

        // No constructor due to circular dependency :)
        public virtual void Initialise(IState otherState, IDurationProvider durationProvider)
        {
            // Helper.AssertIsNotNull(otherState, durationProvider);

            _otherState = otherState;
            _durationProvider = durationProvider;
        }

        public abstract IState OnFired();
        public abstract IState ProcessTimeInterval(float timePassedInS);
    }
}
