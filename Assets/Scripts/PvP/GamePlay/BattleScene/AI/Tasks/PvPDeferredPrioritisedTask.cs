using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Threading;
using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.Tasks
{
    /// <summary>
    /// Sometimes a task is created in response to an event (Eg: A ConstructBuildingTask
    /// in response to a building having been destroyed).  We do not want to start
    /// this task immediately, because then none of the other building destroyed event
    /// handlers have run yet.  
    /// 
    /// Hence, always defer the start of a task to allow any other potential event handlers 
    /// to run before starting this task.  (Destroying a drone station would start a 
    /// ConstructBuildingTask before the DroneConsumerFocusManager
    /// received it's building destroyed event, resulting in a null reference :) )
    /// 
    /// If one task action is deferred, all task actions must be deferred.  (Otherwise,
    /// if we only deferred Start(), then a Stop() could happen before the Start()!)
    /// 
    /// Additionally, do not want to instantly start building, otherwise lasers (like the railgun)
    /// will keep destroying the same building that is instantly being rebuilt,
    /// making those lasers useless :P  Hence wait slightly before trying to rebuild
    /// a building.
    /// </summary>
    public class PvPDeferredPrioritisedTask : IPvPPrioritisedTask
    {
        private readonly IPvPPrioritisedTask _baseTask;
        private readonly IPvPDeferrer _deferrer;
        private readonly IPvPDelayProvider _delayProvider;

        public PvPTaskPriority Priority => _baseTask.Priority;

        public event EventHandler<EventArgs> Completed
        {
            add { _baseTask.Completed += value; }
            remove { _baseTask.Completed -= value; }
        }

        public PvPDeferredPrioritisedTask(IPvPPrioritisedTask baseTask, IPvPDeferrer deferrer, IPvPDelayProvider delayProvider)
        {
            PvPHelper.AssertIsNotNull(baseTask, deferrer, delayProvider);

            _baseTask = baseTask;
            _deferrer = deferrer;
            _delayProvider = delayProvider;
        }

        public void Start()
        {
            _deferrer.Defer(() => _baseTask.Start(), _delayProvider.DelayInS);
        }

        public void Stop()
        {
            _deferrer.Defer(() => _baseTask.Stop(), _delayProvider.DelayInS);
        }

        public override string ToString()
        {
            return _baseTask.ToString();
        }
    }
}
