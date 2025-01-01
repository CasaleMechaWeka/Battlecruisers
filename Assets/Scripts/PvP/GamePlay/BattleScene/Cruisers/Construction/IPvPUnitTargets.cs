using BattleCruisers.Buildables;
using System.Collections.Generic;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Construction
{
    public interface IPvPUnitTargets
    {
        /// <summary>
        /// All ships of a faction that are started and not yet destroyed.
        /// </summary>
        IReadOnlyCollection<ITarget> Ships { get; }

        /// <summary>
        /// All aircraft of a faction that are started and not yet destroyed.
        /// </summary>
        IReadOnlyCollection<ITarget> Aircraft { get; }

        /// <summary>
        /// All ships and aircraft of a faction that are started and not yet destroyed.
        /// </summary>
        IReadOnlyCollection<ITarget> ShipsAndAircraft { get; }
    }
}