using System.Threading.Tasks;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.Tasks.States
{
    public class PvPStoppedState : PvPBaseState
    {
        private readonly IPvPState _inProgressState;

        public PvPStoppedState(IPvPTask task, IPvPCompletedEventEmitter eventEmitter, IPvPState inProgressState)
            : base(task, eventEmitter)
        {
            _inProgressState = inProgressState;
        }

        public override Task<IPvPState> Start()
        {
            _task.Resume();
            return (Task<IPvPState>)_inProgressState;
        }

        public override IPvPState Stop()
        {
            return this;
        }
    }
}