namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.Tasks.States
{
    public class PvPInProgressState : PvPBaseState
    {
        private IPvPState _stoppedState;
        private IPvPState StoppedState
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

        public PvPInProgressState(IPvPTask task, IPvPCompletedEventEmitter eventEmitter)
            : base(task, eventEmitter)
        {
        }

        public override IPvPState Start()
        {
            return this;
        }

        public override IPvPState Stop()
        {
            _task.Stop();
            return StoppedState;
        }
    }
}