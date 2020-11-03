using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Scenes.Test.Utilities;
using NSubstitute;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace BattleCruisers.Scenes.Test
{
    public class BurstFireTestsGod : TestGodBase
	{
        public BarrelController barrel1, barrel2, barrel3;
		public GameObject target1, target2, target3;

        protected override List<GameObject> GetGameObjects()
        {
            return new List<GameObject>()
            {
                barrel1.gameObject,
                barrel2.gameObject,
                barrel3.gameObject,
                target1,
                target2,
                target3
            };
        }

        protected override async Task SetupAsync(Helper helper)
        {
            await InitialisePairAsync(helper, barrel1, target1);
            await InitialisePairAsync(helper, barrel2, target2);
            await InitialisePairAsync(helper, barrel3, target3);
        }

        private async Task InitialisePairAsync(Helper helper, BarrelController barrel, GameObject targetGameObject)
		{
			barrel.StaticInitialise();
			
            ITarget target = Substitute.For<ITarget>();
			Vector2 targetPosition = targetGameObject.transform.position;
			target.Position.Returns(targetPosition);
			barrel.Target = target;
			
            IBarrelControllerArgs barrelControllerArgs = helper.CreateBarrelControllerArgs(barrel, _updaterProvider.PerFrameUpdater);
            await barrel.InitialiseAsync(barrelControllerArgs);
		}
	}
}
