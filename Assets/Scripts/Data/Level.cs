using System.Collections.Generic;
using BattleCruisers.Data.Models.PrefabKeys;

namespace BattleCruisers.Data
{
    // FELIX  Eventually add:
    // + Loot given on accomplishment
    // + Level specific graphics (ie, background image, lighting perhaps?)
    // + AI playing style (once an AI exists :P)
    public class Level : ILevel
    {
        public string Name { get; private set; }
        public Loadout AiLoadout { get; private set; }
        public IList<IPrefabKey> BuildOrder { get; private set; }

        public Level(string name, Loadout aiLoadout, IList<IPrefabKey> buildOrder)
		{
			Name = name;
			AiLoadout = aiLoadout;
            BuildOrder = buildOrder;
		}
	}
}
