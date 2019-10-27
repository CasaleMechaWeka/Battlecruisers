using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Offensive;
using BattleCruisers.Buildables.Buildings.Tactical.Shields;
using BattleCruisers.Cruisers;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets.TargetFinders.Filters;
using NSubstitute;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test.Offensive
{
    public class NukeVsCruiserTestGod : CameraToggleTestGod
	{
        private Cruiser _cruiser;
        private ShieldGenerator _shield;
        private NukeLauncherController _launcher;

        protected override List<GameObject> GetGameObjects()
        {
            _cruiser = FindObjectOfType<Cruiser>();
            Assert.IsNotNull(_cruiser);

            _shield = FindObjectOfType<ShieldGenerator>();
            Assert.IsNotNull(_shield);

            _launcher = FindObjectOfType<NukeLauncherController>();
            Assert.IsNotNull(_launcher);

            return new List<GameObject>()
            {
                _cruiser.GameObject,
                _shield.GameObject,
                _launcher.GameObject
            };
        }

        protected override void Setup(Helper helper)
        {
            // Setup cruiser
            helper.SetupCruiser(_cruiser);

            // Setup shield
            helper.InitialiseBuilding(_shield, Faction.Reds);
            _shield.StartConstruction();

			// Setup nuke launcher
			IExactMatchTargetFilter targetFilter = Substitute.For<IExactMatchTargetFilter>();
			targetFilter.IsMatch(_cruiser).Returns(true);
            ITargetFactories targetFactories = helper.CreateTargetFactories(_cruiser.GameObject, exactMatchTargetFilter: targetFilter);

            helper.InitialiseBuilding(_launcher, Faction.Blues, enemyCruiser: _cruiser, targetFactories: targetFactories);
			_launcher.StartConstruction();
		}
	}
}
