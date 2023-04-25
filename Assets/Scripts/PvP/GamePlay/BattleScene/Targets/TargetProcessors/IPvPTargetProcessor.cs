using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;


namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetProcessors
{
    /// <summary>
    /// Ranks all targets, and assigns the highest ranked target to ITargetConsumers.
    /// </summary>
    public interface IPvPTargetProcessor : IPvPManagedDisposable
    {
        /// <exception cref="ArgumentException">If the target consumer is already added.</exception>
        void AddTargetConsumer(IPvPTargetConsumer targetConsumer);

        /// <exception cref="ArgumentException">If the target consumer was not added first.</exception>
        void RemoveTargetConsumer(IPvPTargetConsumer targetConsumer);
    }
}

