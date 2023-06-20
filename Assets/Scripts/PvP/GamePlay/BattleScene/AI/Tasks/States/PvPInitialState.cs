using System;
using System.Threading.Tasks;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.Tasks.States
{
    public class PvPInitialState : PvPBaseState
    {
        public PvPInitialState(IPvPTask task, IPvPCompletedEventEmitter eventEmitter)
            : base(task, eventEmitter)
        {
        }

        public override async Task<IPvPState> Start()
        {
            var isStart = await _task.Start();
            if (isStart)
            {
                return new PvPInProgressState(_task, _eventEmitter);
            }
            else
            {
                return new PvPCompletedState(_task, _eventEmitter);
            }
        }

        public override IPvPState Stop()
        {
            return this;
        }

        public override IPvPState OnCompleted()
        {
            throw new Exception("Should never complete from the InitialState :(");
        }
    }
}