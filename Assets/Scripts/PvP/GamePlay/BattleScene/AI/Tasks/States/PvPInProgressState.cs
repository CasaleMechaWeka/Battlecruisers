using BattleCruisers.AI.Tasks;
using BattleCruisers.AI.Tasks.States;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.Tasks.States
{
    public class PvPInProgressState : PvPBaseState
    {
        private IState _stoppedState;
        private IState StoppedState
        {
            get
            {
                // Laziliy initialise, because will not be needed unless the task is stopped.
                if (_stoppedState == null)
                {
                    _stoppedState = new PvPStoppedState(_task, _eventEmitter, this);
                }
                return _stoppedState;
            }
        }

        public PvPInProgressState(ITask task, ICompletedEventEmitter eventEmitter)
            : base(task, eventEmitter)
        {
        }

        /*        public override Task<IState> Start()
                {
                    return (Task<IState>)(IState)this;
                }*/

        public override IState Start()
        {
            return this;
        }
        public override IState Stop()
        {
            _task.Stop();
            return StoppedState;
        }
    }
}