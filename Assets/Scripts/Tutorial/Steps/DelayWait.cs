using System;
using BattleCruisers.Utils.Threading;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Steps
{
    /// <summary>
    /// Completed when the specified amount of time has passed.
    /// </summary>
    // FELIX  Test :D
    public class DelayWait : TutorialStep
    {
        private readonly IVariableDelayDeferrer _defferer;
        private readonly float _waitTimeInS;

        private const int MIN_WAIT_TIME_IN_S = 0;

        public DelayWait(ITutorialStepArgs args, IVariableDelayDeferrer defferer, float waitTimeInS)
            : base(args)
        {
            Assert.IsNotNull(defferer);
            Assert.IsTrue(waitTimeInS >= MIN_WAIT_TIME_IN_S);

            _defferer = defferer;
            _waitTimeInS = waitTimeInS;
        }

        public override void Start(Action completionCallback)
        {
            base.Start(completionCallback);
            _defferer.Defer(OnCompleted, _waitTimeInS);
        }
    }
}
