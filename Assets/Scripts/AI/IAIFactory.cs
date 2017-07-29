using System.Collections.Generic;
using BattleCruisers.Cruisers;
using BattleCruisers.Data.PrefabKeys;

namespace BattleCruisers.AI
{
    public interface IAIFactory
    {
        void CreateAI(ICruiserController aiCruiser, IList<IPrefabKey> buildOrder);
	}
}