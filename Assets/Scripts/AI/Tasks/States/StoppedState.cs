namespace BattleCruisers.AI.Tasks.States
{
    public class StoppedState : BaseState
    {
        private readonly BaseState _inProgressState;

        public StoppedState(ITask task, PrioritisedTask eventEmitter, BaseState inProgressState)
            : base(task, eventEmitter)
        {
            _inProgressState = inProgressState;
        }

        public override BaseState Start()
        {
            _task.Resume();
            return _inProgressState;
        }

        public override BaseState Stop()
        {
            return this;
        }
    }
}