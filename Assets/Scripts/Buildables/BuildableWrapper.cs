using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables
{
	public abstract class BuildableWrapper<TBuildable> : MonoBehaviour where TBuildable : Buildable
	{
        public TBuildable Buildable { get; private set; }

		public void Initialise()
		{
			Buildable = gameObject.GetComponentInChildren<TBuildable>();
			Assert.IsNotNull(Buildable);
		}
	}
}

