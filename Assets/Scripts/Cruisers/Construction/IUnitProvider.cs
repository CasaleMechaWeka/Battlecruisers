using BattleCruisers.Buildables.Units;
using System.Collections.Generic;

namespace BattleCruisers.Cruisers.Construction
{
    public interface IUnitProvider
    {
        /// <summary>
        /// Units that have been started and not destroyed.
        /// </summary>
        IReadOnlyCollection<IUnit> AliveUnits { get; }
    }
}