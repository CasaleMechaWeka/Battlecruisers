using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Tactical;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Fog;
using BattleCruisers.Tests.Utils.Extensions;
using BattleCruisers.Utils.PlatformAbstractions;
using NSubstitute;
using NUnit.Framework;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.Cruisers.Fog
{
    public class FogOfWarManagerTests
	{
		private IGameObject _fog;
        private IFogVisibilityDecider _visibilityDecider;
		private ICruiserController _friendlyCruiser, _enemyCruiser;
        private ISpySatelliteLauncher _satelliteLauncher;
        private IStealthGenerator _stealthGenerator;
        private IBuilding _randomBuilding;

		[SetUp]
		public void SetuUp()
		{
			UnityAsserts.Assert.raiseExceptions = true;
   
            _fog = Substitute.For<IGameObject>();
            _visibilityDecider = Substitute.For<IFogVisibilityDecider>();
            _friendlyCruiser = Substitute.For<ICruiserController>();
            _enemyCruiser = Substitute.For<ICruiserController>();
            _satelliteLauncher = Substitute.For<ISpySatelliteLauncher>();
            _stealthGenerator = Substitute.For<IStealthGenerator>();
            _randomBuilding = Substitute.For<IBuilding>();

            new FogOfWarManager(_fog, _visibilityDecider, _friendlyCruiser, _enemyCruiser);
		}

        #region Friendly cruiser building completed
        [Test]
		public void FriendlyStealthGeneratorBuilt()
        {
            _visibilityDecider
                .ShouldFogBeVisible(numOfFriendlyStealthGenerators: 1, numOfEnemySpySatellites: 0)
                .Returns(true);
            BuildStealthGenerator();
            _fog.Received().IsVisible = true;
        }

        [Test]
		public void SecondFriendlyStealthGeneratorBuilt()
		{
            // First stealth generator
            _visibilityDecider
                .ShouldFogBeVisible(numOfFriendlyStealthGenerators: 1, numOfEnemySpySatellites: 0)
                .Returns(true);
            BuildStealthGenerator();
            _fog.Received().IsVisible = true;

            // Second stealth generator
            _fog.ClearReceivedCalls();
            _visibilityDecider
                .ShouldFogBeVisible(numOfFriendlyStealthGenerators: 2, numOfEnemySpySatellites: 0)
                .Returns(true);
            BuildStealthGenerator();
            _fog.Received().IsVisible = true;
		}

        [Test]
		public void RandomFriendlyBuildingBuilt()
		{
            _friendlyCruiser.CompleteConstructingBuliding(_randomBuilding);
            _visibilityDecider.DidNotReceiveWithAnyArgs().ShouldFogBeVisible(default, default);
		}
		#endregion Friendly cruiser building completed

		[Test]
		public void FriendlyStealthGeneratorDestroyed()
        {
            // Build generator
            _visibilityDecider
                .ShouldFogBeVisible(numOfFriendlyStealthGenerators: 1, numOfEnemySpySatellites: 0)
                .Returns(true);
            BuildStealthGenerator();
            _fog.Received().IsVisible = true;

            // Destroy generator
            _visibilityDecider
                .ShouldFogBeVisible(numOfFriendlyStealthGenerators: 0, numOfEnemySpySatellites: 0)
                .Returns(false); DestroyStealthGenerator();
            _fog.Received().IsVisible = false;
        }

        #region Enemy cruiser building completed
        [Test]
		public void EnemySpySatelliteBuilt()
        {
            _visibilityDecider
                .ShouldFogBeVisible(numOfFriendlyStealthGenerators: 0, numOfEnemySpySatellites: 1)
                .Returns(false);
            BuildSpySatellite();
            _fog.Received().IsVisible = false;
        }

		[Test]
		public void SecondEnemySpySatelliteBuilt()
        {
            // First spy satellite
            _visibilityDecider
                .ShouldFogBeVisible(numOfFriendlyStealthGenerators: 0, numOfEnemySpySatellites: 1)
                .Returns(false);
            BuildSpySatellite();
            _fog.Received().IsVisible = false;

            // Second spy satellite
            _fog.ClearReceivedCalls();
            _visibilityDecider
                .ShouldFogBeVisible(numOfFriendlyStealthGenerators: 0, numOfEnemySpySatellites: 2)
                .Returns(false);
            BuildSpySatellite();
            _fog.Received().IsVisible = false;
        }

		[Test]
		public void RandomEnemyBuildingBuilt()
		{
            _enemyCruiser.CompleteConstructingBuliding(_randomBuilding);
            _visibilityDecider.DidNotReceiveWithAnyArgs().ShouldFogBeVisible(default, default);
		}
		#endregion Enemy cruiser building completed

        [Test]
        public void EnemySpySatelliteDestroyed()
        {
            // Build satellite
            _visibilityDecider
                .ShouldFogBeVisible(numOfFriendlyStealthGenerators: 0, numOfEnemySpySatellites: 1)
                .Returns(false);
            BuildSpySatellite();
            _fog.Received().IsVisible = false;

            // Destroy satellite
            _fog.ClearReceivedCalls();
            _visibilityDecider
                .ShouldFogBeVisible(numOfFriendlyStealthGenerators: 0, numOfEnemySpySatellites: 0)
                .Returns(false);
            DestroySpySatellite();
            _fog.Received().IsVisible = false;
        }

        [Test]
        public void Combo()
        {
            BuildStealthGenerator();
            _visibilityDecider.Received().ShouldFogBeVisible(numOfFriendlyStealthGenerators: 1, numOfEnemySpySatellites: 0);

            BuildSpySatellite();
			_visibilityDecider.Received().ShouldFogBeVisible(numOfFriendlyStealthGenerators: 1, numOfEnemySpySatellites: 1);

            BuildSpySatellite();
			_visibilityDecider.Received().ShouldFogBeVisible(numOfFriendlyStealthGenerators: 1, numOfEnemySpySatellites: 2);

            DestroyStealthGenerator();
			_visibilityDecider.Received().ShouldFogBeVisible(numOfFriendlyStealthGenerators: 0, numOfEnemySpySatellites: 2);

            DestroySpySatellite();
			_visibilityDecider.Received().ShouldFogBeVisible(numOfFriendlyStealthGenerators: 0, numOfEnemySpySatellites: 1);

            BuildStealthGenerator();
			_visibilityDecider.Received().ShouldFogBeVisible(numOfFriendlyStealthGenerators: 1, numOfEnemySpySatellites: 1);
		}

        private void BuildStealthGenerator()
        {
            _friendlyCruiser.CompleteConstructingBuliding(_stealthGenerator);
        }

        private void DestroyStealthGenerator()
		{
			_stealthGenerator.Destroyed += Raise.EventWith(_stealthGenerator, new DestroyedEventArgs(_stealthGenerator));
		}

		private void BuildSpySatellite()
		{
            _enemyCruiser.CompleteConstructingBuliding(_satelliteLauncher);
		}

        private void DestroySpySatellite()
        {
            _satelliteLauncher.Destroyed += Raise.EventWith(_satelliteLauncher, new DestroyedEventArgs(_satelliteLauncher));
        }
	}
}
