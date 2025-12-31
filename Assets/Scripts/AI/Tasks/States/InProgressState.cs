namespace BattleCruisers.AI.Tasks.States
{
    public class InProgressState : BaseState
    {
        private BaseState _stoppedState;
        private BaseState StoppedState
        {
            get
            {
                // Laziliy initialise, because will not be needed unless the task is stopped.
                if (_stoppedState == null)
                {
                    _stoppedState = new StoppedState(_task, _eventEmitter, this);
                }
                return _stoppedState;
            }
        }

        public InProgressState(ITask task, PrioritisedTask eventEmitter)
            : base(task, eventEmitter)
        {
        }

        public override BaseState Start()
        {
            return this;
        }

        public override BaseState Stop()
        {
            _task.Stop();
            return StoppedState;
        }
    }
}