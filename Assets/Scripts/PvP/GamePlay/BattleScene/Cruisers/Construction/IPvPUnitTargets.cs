using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using System.Collections.Generic;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Construction
{
    public interface IPvPUnitTargets
    {
        /// <summary>
        /// All ships of a faction that are started and not yet destroyed.
        /// </summary>
        IReadOnlyCollection<IPvPTarget> Ships { get; }

        /// <summary>
        /// All aircraft of a faction that are started and not yet destroyed.
        /// </summary>
        IReadOnlyCollection<IPvPTarget> Aircraft { get; }

        /// <summary>
        /// All ships and aircraft of a faction that are started and not yet destroyed.
        /// </summary>
        IReadOnlyCollection<IPvPTarget> ShipsAndAircraft { get; }
    }
}