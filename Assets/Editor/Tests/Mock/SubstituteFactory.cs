using BattleCruisers.Utils.Threading;
using NSubstitute;
using NUnit.Framework;
using System;

namespace BattleCruisers.Tests.Mock
{
    public static class SubstituteFactory
    {
        public static IVariableDelayDeferrer CreateVariableDelayDeferrer()
        {
            IVariableDelayDeferrer deferrer = Substitute.For<IVariableDelayDeferrer>();

            // Call deferred action immediately
            deferrer
                .WhenForAnyArgs(def => def.Defer(null, default))
                .Do(callInfo =>
                {
                    Assert.IsTrue(callInfo.Args().Length == 2);
                    Action actionToDefer = callInfo.Args()[0] as Action;
                    Assert.IsNotNull(actionToDefer);
                    actionToDefer.Invoke();
                });

            return deferrer;
        }
    }
}