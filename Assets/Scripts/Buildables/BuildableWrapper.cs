using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.Utils.Localisation;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables
{
    public class BuildableWrapper<TBuildable> : Prefab, IBuildableWrapper<TBuildable> where TBuildable : class, IBuildable
	{
        public TBuildable Buildable { get; private set; }

        public BuildableWrapper<TBuildable> UnityObject => this;

        public override void StaticInitialise(ILocTable commonStrings)
		{
            Buildable = GetComponentInChildren<TBuildable>();
			Assert.IsNotNull(Buildable);

            HealthBarController healthBar = GetComponentInChildren<HealthBarController>();
            Assert.IsNotNull(healthBar);

            Buildable.StaticInitialise(gameObject, healthBar, commonStrings);
		}
	}
}
