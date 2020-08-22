using BattleCruisers.Buildables;
using System.Collections.Generic;

namespace BattleCruisers.Cruisers.Construction
{
    public interface IUnitTargets
    {
        /// <summary>
        /// All ships of a faction that are started and not yet destroyed.
        /// </summary>
        IReadOnlyCollection<ITarget> Ships { get; }

        /// <summary>
        /// All aircraft of a faction that are started and not yet destroyed.
        /// </summary>
        IReadOnlyCollection<ITarget> Aircraft { get; }
    }
}