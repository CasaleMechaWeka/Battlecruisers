using System;

namespace BattleCruisers.Targets.TargetProcessors
{
    public interface ITargetProcessorWrapper : IDisposable
	{
        void StartProvidingTargets();
	}
}
