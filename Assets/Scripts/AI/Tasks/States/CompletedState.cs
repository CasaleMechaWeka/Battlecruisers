namespace BattleCruisers.AI.Tasks.States
{
	public class CompletedState : BaseState
	{
        public CompletedState(IInternalTask task)
			: base(task)
		{
		}

		public override IState Start()
		{
            return this;
		}

		public override IState Stop()
		{
			return this;
		}
	}
}