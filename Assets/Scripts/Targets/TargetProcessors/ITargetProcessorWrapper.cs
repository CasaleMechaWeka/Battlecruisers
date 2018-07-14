using BattleCruisers.Utils;

namespace BattleCruisers.Targets.TargetProcessors
{
    public interface ITargetProcessorWrapper : IManagedDisposable
    {
        void StartProvidingTargets();
	}
}
