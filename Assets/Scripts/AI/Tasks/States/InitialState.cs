using System;

namespace BattleCruisers.AI.Tasks.States
{
    public class InitialState : BaseState
    {
        public InitialState(ITask task, ICompletedEventEmitter eventEmitter)
            : base(task, eventEmitter)
        {
        }

        public override IState Start()
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

        public override IState Stop()
        {
            return this;
        }

        public override IState OnCompleted()
        {
            throw new Exception("Should never complete from the InitialState :(");
        }
    }
}