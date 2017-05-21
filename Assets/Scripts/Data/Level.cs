using System;

namespace BattleCruisers.Data
{
	public interface ILevel
	{
		 Loadout AiLoadout { get; }
	}

	// FELIX  Eventuall add:
	// + Loot given on accomplishment
	// + Level specific graphics (ie, background image, lighting perhaps?)
	// + AI playing style (once an AI exists :P)
	public class Level : ILevel
	{
		public Loadout AiLoadout { get; private set; }

		public Level(Loadout aiLoadout)
		{
			AiLoadout = aiLoadout;
		}
	}
}

