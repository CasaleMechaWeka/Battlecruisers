namespace BattleCruisers.AI.Tasks.States
{
    public abstract class BaseState : IState
	{
		protected readonly ITask _task;
        protected readonly ICompletedEventEmitter _eventEmitter;

        public BaseState(ITask task, ICompletedEventEmitter eventEmitter)
		{
			_task = task;
            _eventEmitter = eventEmitter;
		}

        public abstract IState Start();

        public abstract IState Stop();
		
		public virtual IState OnCompleted()
		{
            return new CompletedState(_task, _eventEmitter);
		}
	}
}