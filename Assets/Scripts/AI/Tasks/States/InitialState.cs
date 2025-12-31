using System;

namespace BattleCruisers.AI.Tasks.States
{
    public class InitialState : BaseState
    {
        public InitialState(ITask task, PrioritisedTask eventEmitter)
            : base(task, eventEmitter)
        {
        }

        public override BaseState Start()
        {
            if (_task.Start())
            {
                return new InProgressState(_task, _eventEmitter);
            }
            else
            {
                return new CompletedState(_task, _eventEmitter);
            }
        }

        public override BaseState Stop()
        {
            return this;
        }

        public override BaseState OnCompleted()
        {
            throw new Exception("Should never complete from the InitialState :(");
        }
    }
}