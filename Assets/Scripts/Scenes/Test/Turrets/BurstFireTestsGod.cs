using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Scenes.Test.Utilities;
using NSubstitute;
using UnityEngine;

namespace BattleCruisers.Scenes.Test
{
    public class BurstFireTestsGod : TestGodBase
	{
        private Helper _helper;
        public BarrelController barrel1, barrel2, barrel3;
		public GameObject target1, target2, target3;

        protected override void Start()
        {
            base.Start();

            _helper = new Helper();

            InitialisePair(barrel1, target1);
            InitialisePair(barrel2, target2);
            InitialisePair(barrel3, target3);
        }

        private void InitialisePair(BarrelController barrel, GameObject targetGameObject)
		{
			barrel.StaticInitialise();
			
            ITarget target = Substitute.For<ITarget>();
			Vector2 targetPosition = targetGameObject.transform.position;
			target.Position.Returns(targetPosition);
			barrel.Target = target;
			
            IBarrelControllerArgs barrelControllerArgs = _helper.CreateBarrelControllerArgs(barrel, _updaterProvider.PerFrameUpdater);
            barrel.InitialiseAsync(barrelControllerArgs);
		}
	}
}
