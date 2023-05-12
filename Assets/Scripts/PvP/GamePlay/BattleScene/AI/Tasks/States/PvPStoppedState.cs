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

        public override IPvPState Start()
        {
            _task.Resume();
            return _inProgressState;
        }

        public override IPvPState Stop()
        {
            return this;
        }
    }
}