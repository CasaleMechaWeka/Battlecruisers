namespace BattleCruisers.AI.Tasks.States
{
	public class InProgressState : BaseState
	{
        private readonly IState _stoppedState;

        public InProgressState(IInternalTask task, IState stoppedState)
			: base(task)
		{
			_stoppedState = stoppedState;
		}

		public override IState Start()
		{
            return this;
		}

		public override IState Stop()
		{
            _task.Stop();
            return _stoppedState;
		}
	}
}