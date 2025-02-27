using BattleCruisers.AI.Tasks;
using BattleCruisers.AI.Tasks.States;
using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.Tasks.States
{
    public class PvPInitialState : PvPBaseState
    {
        public PvPInitialState(IPvPTask task, ICompletedEventEmitter eventEmitter)
            : base(task, eventEmitter)
        {
        }

        /*        public override async Task<IState> Start()
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
                }*/


        public override IState Start()
        {
            //   var isStart = _task.Start();
            if (_task.Start())
            {
                return new PvPInProgressState(_task, _eventEmitter);
            }
            else
            {
                return new PvPCompletedState(_task, _eventEmitter);
            }
        }

        public override IState Stop()
        {
            return this;
        }

        public override IState OnCompleted()
        {
            throw new Exception("Should never complete from the InitialState :(");
        }
    }
}