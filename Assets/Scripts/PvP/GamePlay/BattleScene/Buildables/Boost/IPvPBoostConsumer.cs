using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost
{
    /// <summary>
    /// Consumes boost producer(s).  Simply provides the cumulative boost
    /// of all boost providers.
    /// </summary>
    public interface IPvPBoostConsumer
    {
        float CumulativeBoost { get; }

        event EventHandler BoostChanged;

        void AddBoostProvider(IPvPBoostProvider boostProvider);
        void RemoveBoostProvider(IPvPBoostProvider boostProvider);
    }
}

