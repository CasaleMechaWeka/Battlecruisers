namespace BattleCruisers.AI.Tasks.States
{
    public class StoppedState : BaseState
    {
        private readonly IState _inProgressState;

        public StoppedState(ITask task, ICompletedEventEmitter eventEmitter, IState inProgressState)
            : base(task, eventEmitter)
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