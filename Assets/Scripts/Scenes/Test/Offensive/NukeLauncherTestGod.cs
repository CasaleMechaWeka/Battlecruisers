using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Buildings.Offensive;
using BattleCruisers.Cruisers;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets.TargetFinders.Filters;
using NSubstitute;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Offensive
{
    public class NukeLauncherTestGod : CameraToggleTestGod
	{
        private AirFactory _baseTarget;
        private NukeLauncherController _launcher;

        protected override List<GameObject> GetGameObjects()
        {
            _baseTarget = FindObjectOfType<AirFactory>();
			_launcher = FindObjectOfType<NukeLauncherController>();

            return new List<GameObject>()
            {
                _baseTarget.GameObject,
                _launcher.GameObject
            };
        }

        protected override void Setup(Helper helper)
        {
			// Setup targets
            helper.InitialiseBuilding(_baseTarget);

            DroneStation[] _targets = FindObjectsOfType<DroneStation>();
            foreach (DroneStation target in _targets)
            {
                helper.InitialiseBuilding(target);
            }

			// Setup nuke launcher
			ICruiser enemyCruiser = helper.CreateCruiser(_baseTarget.GameObject);
			IExactMatchTargetFilter targetFilter = Substitute.For<IExactMatchTargetFilter>();
			targetFilter.IsMatch(_baseTarget).Returns(true);
            ITargetFactories targetFactories = helper.CreateTargetFactories(_baseTarget.GameObject, exactMatchTargetFilter: targetFilter);

            helper.InitialiseBuilding(_launcher, enemyCruiser: enemyCruiser, targetFactories: targetFactories);
			_launcher.StartConstruction();
		}
	}
}
