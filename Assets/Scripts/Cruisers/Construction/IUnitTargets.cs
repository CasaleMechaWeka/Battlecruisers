using BattleCruisers.Buildables;
using System.Collections.Generic;

namespace BattleCruisers.Cruisers.Construction
{
    public interface IUnitTargets
    {
        // FELIX  This is the bug :)  Should include ships that have been started, not just completed :)
        /// <summary>
        /// All ships of a faction that are completed and not yet destroyed.
        /// </summary>
        IReadOnlyCollection<ITarget> Ships { get; }

        /// <summary>
        /// All aircraft of a faction that are completed and not yet destroyed.
        /// </summary>
        IReadOnlyCollection<ITarget> Aircraft { get; }
    }
}