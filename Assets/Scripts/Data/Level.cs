using BattleCruisers.Data.Models.PrefabKeys;

namespace BattleCruisers.Data
{
    public class Level : ILevel
    {
		public int Num { get; private set; }
        public string Name { get; private set; }
        public IPrefabKey Hull { get; private set; }

        public Level(int num, string name, IPrefabKey hull)
		{
			Num = num;
            Name = name;
            Hull = hull;
		}
	}
}
