using System;

namespace BattleCruisers.AI.Tasks.States
{
	public class CompletedState : BaseState
	{
        public CompletedState(IInternalTask task, ICompletedEventEmitter eventEmitter)
            : base(task, eventEmitter)
		{
            _eventEmitter.EmitCompletedEvent();
		}

		public override IState Start()
		{
            _eventEmitter.EmitCompletedEvent();
            return this;
		}

		public override IState Stop()
		{
			return this;
		}

        public override IState OnCompleted()
        {
			throw new Exception("Should never complete from the CompletedState :(");
		}
	}
}