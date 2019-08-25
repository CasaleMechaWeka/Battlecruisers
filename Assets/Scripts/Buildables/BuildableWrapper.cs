using BattleCruisers.UI.BattleScene.ProgressBars;
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
            Buildable = GetComponentInChildren<TBuildable>();
			Assert.IsNotNull(Buildable);

            HealthBarController healthBar = GetComponentInChildren<HealthBarController>();
            Assert.IsNotNull(healthBar);

            Buildable.StaticInitialise(gameObject, healthBar);
		}
	}
}
