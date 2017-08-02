using System.Collections.Generic;
using BattleCruisers.Cruisers;
using BattleCruisers.Data.Models.PrefabKeys;

namespace BattleCruisers.AI
{
    public interface IAIFactory
    {
        void CreateBasicAI(ICruiserController aiCruiser, IList<IPrefabKey> buildOrder);
	}
}