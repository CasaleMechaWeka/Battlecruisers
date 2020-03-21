using BattleCruisers.UI.BattleScene.ProgressBars;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Units.Ships
{
    public abstract class AnimatedShipController : ShipController
	{
        public Animator propulsionAnimation;

        public override void StaticInitialise(GameObject parent, HealthBarController healthBar)
        {
            base.StaticInitialise(parent, healthBar);
            Assert.IsNotNull(propulsionAnimation);
        }

        protected override void OnBuildableCompleted()
        {
            base.OnBuildableCompleted();
            propulsionAnimation.gameObject.SetActive(true);
        }

        public override void StartMoving()
        {
            base.StartMoving();
            propulsionAnimation.speed = 1;
        }

        public override void StopMoving()
        {
            base.StopMoving();
            propulsionAnimation.speed = 0;
        }

        protected override void Deactivate()
        {
            base.Deactivate();
            propulsionAnimation.gameObject.SetActive(false);
        }
    }
}
