using System;
using System.Threading.Tasks;
// using System.Threading.Tasks;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.Tasks.States
{
    public class PvPCompletedState : PvPBaseState
    {
        public PvPCompletedState(IPvPTask task, IPvPCompletedEventEmitter eventEmitter)
            : base(task, eventEmitter)
        {
            _eventEmitter.EmitCompletedEvent();
        }

        /*        public override Task<IPvPState> Start()
                {
                    _eventEmitter.EmitCompletedEvent();
                    return (Task<IPvPState>)(IPvPState)this;
                }*/

        public override IPvPState Start()
        {
            _eventEmitter.EmitCompletedEvent();
            return this;
        }

        public override IPvPState Stop()
        {
            return this;
        }

        public override IPvPState OnCompleted()
        {
            throw new Exception("Should never complete from the CompletedState :(");
        }
    }
}