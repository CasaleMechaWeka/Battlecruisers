namespace BattleCruisers.Data
{
    public interface ILevel
	{
		Loadout AiLoadout { get; }
		string Name { get; }
	}

	// FELIX  Eventuall add:
	// + Loot given on accomplishment
	// + Level specific graphics (ie, background image, lighting perhaps?)
	// + AI playing style (once an AI exists :P)
	public class Level : ILevel
	{
		public string Name { get; private set; }
		public Loadout AiLoadout { get; private set; }

		public Level(string name, Loadout aiLoadout)
		{
			Name = name;
			AiLoadout = aiLoadout;
		}
	}
}
