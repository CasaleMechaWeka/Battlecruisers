using BattleCruisers.Scenes.Test.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Scenes.Test
{
    public class MissileMovingTargetTestsGod : MissileTestsGod
	{
        private TestAircraftController _target;

        public List<Vector2> targetPatrolPoints;

        protected override List<GameObject> GetGameObjects()
        {
            List<GameObject> gameObjects = base.GetGameObjects();

            _target = FindObjectOfType<TestAircraftController>();
            gameObjects.Add(_target.GameObject);

            return gameObjects;
        }

        protected override void Setup(Helper helper)
        {
			_target.PatrolPoints = targetPatrolPoints;
            helper.InitialiseUnit(_target);
			_target.StartConstruction();

			SetupMissiles(_target);
		}
	}
}
