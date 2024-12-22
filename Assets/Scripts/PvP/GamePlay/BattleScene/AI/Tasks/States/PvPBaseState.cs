using System.Threading.Tasks;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.Tasks.States
{
    public abstract class PvPBaseState : IPvPState
    {
        protected readonly IPvPTask _task;
        protected readonly IPvPCompletedEventEmitter _eventEmitter;

        public PvPBaseState(IPvPTask task, IPvPCompletedEventEmitter eventEmitter)
        {
            _task = task;
            _eventEmitter = eventEmitter;
        }

        public abstract IPvPState Start();

        public abstract IPvPState Stop();

        public virtual IPvPState OnCompleted()
        {
            return new PvPCompletedState(_task, _eventEmitter);
        }
    }
}