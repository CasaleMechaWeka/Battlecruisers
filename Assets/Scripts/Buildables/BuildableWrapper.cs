using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables
{
    public abstract class BuildableWrapper<TBuildable> : MonoBehaviour, IBuildableWrapper<TBuildable> where TBuildable : Buildable
	{
        public TBuildable Buildable { get; private set; }

        public BuildableWrapper<TBuildable> UnityObject { get { return this; } }

        public void Initialise()
		{
			Buildable = gameObject.GetComponentInChildren<TBuildable>();
			Assert.IsNotNull(Buildable);
		}
	}
}

