namespace BattleCruisers.Buildables.Boost
{
    public class Boostable : IBoostable
	{
		public float BoostMultiplier { get; set; }

        public Boostable(float initialMultiplier)
        {
            BoostMultiplier = initialMultiplier;
        }
	}
}
