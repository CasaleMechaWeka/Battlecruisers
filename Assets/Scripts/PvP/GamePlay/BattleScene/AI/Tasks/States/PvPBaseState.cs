using BattleCruisers.AI.Tasks.States;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.Tasks.States
{
    public abstract class PvPBaseState : IState
    {
        protected readonly IPvPTask _task;
        protected readonly IPvPCompletedEventEmitter _eventEmitter;

        public PvPBaseState(IPvPTask task, IPvPCompletedEventEmitter eventEmitter)
        {
            _task = task;
            _eventEmitter = eventEmitter;
        }

        public abstract IState Start();

        public abstract IState Stop();

        public virtual IState OnCompleted()
        {
            return new PvPCompletedState(_task, _eventEmitter);
        }
    }
}