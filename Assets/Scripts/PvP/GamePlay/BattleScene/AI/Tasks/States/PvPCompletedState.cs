using BattleCruisers.AI.Tasks;
using BattleCruisers.AI.Tasks.States;
using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.Tasks.States
{
    public class PvPCompletedState : PvPBaseState
    {
        public PvPCompletedState(ITask task, ICompletedEventEmitter eventEmitter)
            : base(task, eventEmitter)
        {
            _eventEmitter.EmitCompletedEvent();
        }

        /*        public override Task<IState> Start()
                {
                    _eventEmitter.EmitCompletedEvent();
                    return (Task<IState>)(IState)this;
                }*/

        public override IState Start()
        {
            _eventEmitter.EmitCompletedEvent();
            return this;
        }

        public override IState Stop()
        {
            return this;
        }

        public override IState OnCompleted()
        {
            throw new Exception("Should never complete from the CompletedState :(");
        }
    }
}