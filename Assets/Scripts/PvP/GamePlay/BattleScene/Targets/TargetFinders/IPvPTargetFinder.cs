using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetDetectors;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetFinders
{
    /// <summary>
    /// Finds targets to feed to a ITargeProcessor.
    /// </summary>
    public interface IPvPTargetFinder : IPvPManagedDisposable
    {
        // When a target is found (eg, started being built, or comes within range)
        event EventHandler<PvPTargetEventArgs> TargetFound;

        // When an existing target is lost (eg, because it is destroyed or
        // moves out of range)
        event EventHandler<PvPTargetEventArgs> TargetLost;
    }
}