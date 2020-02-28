using BattleCruisers.UI.BattleScene.ProgressBars;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Units.Ships
{
    public abstract class AnimatedShipController : ShipController
	{
        public GameObject propulsionAnimation;

        public override void StaticInitialise(GameObject parent, HealthBarController healthBar)
        {
            base.StaticInitialise(parent, healthBar);
            Assert.IsNotNull(propulsionAnimation);
        }

        protected override void OnBuildableCompleted()
        {
            base.OnBuildableCompleted();
            propulsionAnimation.SetActive(true);
        }

        protected override void Deactivate()
        {
            base.Deactivate();
            propulsionAnimation.SetActive(false);
        }
    }
}
