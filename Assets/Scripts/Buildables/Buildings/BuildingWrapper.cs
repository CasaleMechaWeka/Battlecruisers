using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings
{
    public class BuildingWrapper : BuildableWrapper
	{
		public Building Building { get; private set; }

		public override void Initialise()
		{
			Building = gameObject.GetComponentInChildren<Building>();
			Assert.IsNotNull(Building);
		}
	}
}
