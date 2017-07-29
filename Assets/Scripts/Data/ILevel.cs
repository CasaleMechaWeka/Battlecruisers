using System.Collections.Generic;
using BattleCruisers.Data.Models.PrefabKeys;

namespace BattleCruisers.Data
{
    public interface ILevel
    {
        Loadout AiLoadout { get; }
        string Name { get; }
        IList<IPrefabKey> BuildOrder { get; }
    }
}
