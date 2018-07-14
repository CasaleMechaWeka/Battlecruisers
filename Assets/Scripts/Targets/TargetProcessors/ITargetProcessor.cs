using BattleCruisers.Utils;

namespace BattleCruisers.Targets.TargetProcessors
{
    /// <summary>
    /// Ranks all targets, and assigns the highest ranked target to ITargetConsumers.
    /// </summary>
    public interface ITargetProcessor : IManagedDisposable
    {
        void StartProcessingTargets();

		/// <exception cref="ArgumentException">If the target consumer is already added.</exception>
		void AddTargetConsumer(ITargetConsumer targetConsumer);

		/// <exception cref="ArgumentException">If the target consumer was not added first.</exception>
		void RemoveTargetConsumer(ITargetConsumer targetConsumer);
	}
}
