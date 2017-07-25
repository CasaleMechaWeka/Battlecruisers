namespace BattleCruisers.AI.Tasks.States
{
	public class InProgressState : BaseState
	{
        private IState _stoppedState;
        private IState StoppedState
        {
            get
            {
                if (_stoppedState == null)
                {
                    _stoppedState = new StoppedState(_task, _eventEmitter, this);
                }
                return _stoppedState;
            }
        }

        public InProgressState(IInternalTask task, ICompletedEventEmitter eventEmitter)
            : base(task, eventEmitter)
		{
		}

		public override IState Start()
		{
            return this;
		}

		public override IState Stop()
		{
            _task.Stop();
            return StoppedState;
		}
	}
}