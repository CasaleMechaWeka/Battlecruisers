using BattleCruisers.Utils;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval.States
{
    public abstract class State : IState
    {
        protected IState _otherState;
        protected IDurationProvider _durationProvider;

        public abstract bool ShouldFire { get; }

        // No constructor due to circular dependency :)
        public virtual void Initialise(IState otherState, IDurationProvider durationProvider)
        {
            Helper.AssertIsNotNull(otherState, durationProvider);

            _otherState = otherState;
            _durationProvider = durationProvider;
        }

        public abstract IState OnFired();
        public abstract IState ProcessTimeInterval(float timePassedInS);
    }
}
