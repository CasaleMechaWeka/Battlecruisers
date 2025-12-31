namespace BattleCruisers.AI.Tasks.States
{
	public abstract class BaseState
	{
		protected readonly ITask _task;
		protected readonly PrioritisedTask _eventEmitter;

		public BaseState(ITask task, PrioritisedTask eventEmitter)
		{
			_task = task;
			_eventEmitter = eventEmitter;
		}

		public abstract BaseState Start();

		public abstract BaseState Stop();

		public virtual BaseState OnCompleted()
		{
			return new CompletedState(_task, _eventEmitter);
		}
	}
}