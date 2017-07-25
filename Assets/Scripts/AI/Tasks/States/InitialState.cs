namespace BattleCruisers.AI.Tasks.States
{
    public class InitialState : BaseState
    {
        private readonly IState _inProgressState;

        public InitialState(IInternalTask task, IState inProgressState)
            : base(task)
        {
            _inProgressState = inProgressState;
        }

        public override IState Start()
        {
            _task.Start();
            return _inProgressState;
        }

        public override IState Stop()
        {
            return this;
        }
    }
}