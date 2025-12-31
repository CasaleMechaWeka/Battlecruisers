using System;

namespace BattleCruisers.AI.Tasks.States
{
	public class CompletedState : BaseState
	{
		public CompletedState(ITask task, PrioritisedTask eventEmitter)
			: base(task, eventEmitter)
		{
			_eventEmitter.EmitCompletedEvent();
		}

		public override BaseState Start()
		{
			_eventEmitter.EmitCompletedEvent();
			return this;
		}

		public override BaseState Stop()
		{
			return this;
		}

		public override BaseState OnCompleted()
		{
			throw new Exception("Should never complete from the CompletedState :(");
		}
	}
}