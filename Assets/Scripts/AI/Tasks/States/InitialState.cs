using System;

namespace BattleCruisers.AI.Tasks.States
{
    public class InitialState : BaseState
    {
        public InitialState(IInternalTask task, ICompletedEventEmitter eventEmitter)
            : base(task, eventEmitter)
        {
        }

        public override IState Start()
        {
            _task.Start();
            return new InProgressState(_task, _eventEmitter);
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