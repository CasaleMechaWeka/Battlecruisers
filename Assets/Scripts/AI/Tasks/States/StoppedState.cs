namespace BattleCruisers.AI.Tasks.States
{
    public class StoppedState : BaseState
    {
        private readonly IState _inProgressState;

        public StoppedState(IInternalTask task, IState inProgressState)
            : base(task)
        {
            _inProgressState = inProgressState;
        }

        public override IState Start()
        {
            _task.Resume();
            return _inProgressState;
        }

        public override IState Stop()
        {
            return this;
        }
    }
}