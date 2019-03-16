using BattleCruisers.Utils;
using BattleCruisers.Utils.Threading;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.AI.Tasks
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
    public class DeferredPrioritisedTask : IPrioritisedTask
    {
        private readonly IPrioritisedTask _baseTask;
        private readonly IVariableDelayDeferrer _deferrer;
        private readonly float _delayInS;

        public const float DEFAULT_DELAY_IN_S = 1;
        private const float MIN_DELAY_IN_S = 0.1f;

        public TaskPriority Priority => _baseTask.Priority;

        public event EventHandler<EventArgs> Completed
        {
            add { _baseTask.Completed += value; }
            remove { _baseTask.Completed -= value; }
        }

        public DeferredPrioritisedTask(IPrioritisedTask baseTask, IVariableDelayDeferrer deferrer, float delayInS = DEFAULT_DELAY_IN_S)
        {
            Helper.AssertIsNotNull(baseTask, deferrer);
            Assert.IsTrue(delayInS >= MIN_DELAY_IN_S);

            _baseTask = baseTask;
            _deferrer = deferrer;
            _delayInS = delayInS;
        }

        public void Start()
        {
            _deferrer.Defer(() => _baseTask.Start(), _delayInS);
        }

        public void Stop()
        {
            _deferrer.Defer(() => _baseTask.Stop(), _delayInS);
        }

        public override string ToString()
        {
            return _baseTask.ToString();
        }
    }
}
