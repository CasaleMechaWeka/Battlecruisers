using BattleCruisers.AI.Tasks;
using BattleCruisers.AI.Tasks.States;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.Tasks.States
{
    public class PvPStoppedState : PvPBaseState
    {
        private readonly IState _inProgressState;

        public PvPStoppedState(IPvPTask task, ICompletedEventEmitter eventEmitter, IState inProgressState)
            : base(task, eventEmitter)
        {
            _inProgressState = inProgressState;
        }

        /*        public override Task<IState> Start()
                {
                    _task.Resume();
                    return (Task<IState>)_inProgressState;
                }*/

        public override IState Start()
        {
            _task.Resume();
            return _inProgressState;
        }

        public override IState Stop()
        {
            return this;
        }
    }
}