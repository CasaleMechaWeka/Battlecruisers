using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables
{
    public class BuildableWrapper<TBuildable> : MonoBehaviour, IBuildableWrapper<TBuildable> where TBuildable : class, IBuildable
	{
        public TBuildable Buildable { get; private set; }

        public BuildableWrapper<TBuildable> UnityObject => this;

        public void Initialise()
		{
            Buildable = gameObject.GetComponentInChildren<TBuildable>();
			Assert.IsNotNull(Buildable);
            Buildable.StaticInitialise();
		}
	}
}
