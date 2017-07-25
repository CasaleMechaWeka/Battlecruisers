namespace BattleCruisers.AI.Tasks.States
{
    public abstract class BaseState : IState
	{
		protected readonly IInternalTask _task;

        public BaseState(IInternalTask task)
		{
			_task = task;
		}

        public abstract IState Start();

        public abstract IState Stop();
		
		public IState OnCompleted()
		{
			return new CompletedState(_task);
		}
	}
}