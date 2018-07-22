using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Tactical;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Fog;
using BattleCruisers.Tests.Utils.Extensions;
using NSubstitute;
using NUnit.Framework;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.Cruisers
{
    public class FogOfWarManagerTests
	{
		private IFogOfWar _fog;
		private ICruiserController _friendlyCruiser, _enemyCruiser;
        private ISpySatelliteLauncher _satelliteLauncher;
        private IStealthGenerator _stealthGenerator;
        private IBuilding _randomBuilding;

		[SetUp]
		public void SetuUp()
		{
			UnityAsserts.Assert.raiseExceptions = true;
   
            _fog = Substitute.For<IFogOfWar>();
            _friendlyCruiser = Substitute.For<ICruiserController>();
            _enemyCruiser = Substitute.For<ICruiserController>();
            _satelliteLauncher = Substitute.For<ISpySatelliteLauncher>();
            _stealthGenerator = Substitute.For<IStealthGenerator>();
            _randomBuilding = Substitute.For<IBuilding>();

            new FogOfWarManager(_fog, _friendlyCruiser, _enemyCruiser);
		}

        #region Friendly cruiser building completed
        [Test]
		public void FriendlyStealthGeneratorBuilt_Updates()
        {
            BuildStealthGenerator();
            _fog.Received().UpdateIsEnabled(numOfFriendlyStealthGenerators: 1, numOfEnemySpySatellites: 0);
        }

        [Test]
		public void SecondFriendlyStealthGeneratorBuilt_Updates()
		{
            BuildStealthGenerator();
			_fog.Received().UpdateIsEnabled(numOfFriendlyStealthGenerators: 1, numOfEnemySpySatellites: 0);

			BuildStealthGenerator();
			_fog.Received().UpdateIsEnabled(numOfFriendlyStealthGenerators: 2, numOfEnemySpySatellites: 0);
		}

        [Test]
		public void RandomFriendlyBuildingBuilt_DoesNotUpdate()
		{
            _friendlyCruiser.CompleteConstructingBuliding(_randomBuilding);
            _fog.DidNotReceiveWithAnyArgs().UpdateIsEnabled(numOfFriendlyStealthGenerators: -99, numOfEnemySpySatellites: -99);
		}
		#endregion Friendly cruiser building completed

		[Test]
		public void FriendlyStealthGeneratorDestroyed_Updates()
        {
            // Build generator
            BuildStealthGenerator();
            _fog.Received().UpdateIsEnabled(numOfFriendlyStealthGenerators: 1, numOfEnemySpySatellites: 0);

            // Destroy generator
            DestroyStealthGenerator();
            _fog.Received().UpdateIsEnabled(numOfFriendlyStealthGenerators: 0, numOfEnemySpySatellites: 0);
        }

        #region Enemy cruiser building completed
        [Test]
		public void EnemySpySatelliteBuilt_Updates()
        {
			BuildSpySatellite();
            _fog.Received().UpdateIsEnabled(numOfFriendlyStealthGenerators: 0, numOfEnemySpySatellites: 1);
        }

		[Test]
		public void SecondEnemySpySatelliteBuilt_Updates()
        {
			BuildSpySatellite();
            _fog.Received().UpdateIsEnabled(numOfFriendlyStealthGenerators: 0, numOfEnemySpySatellites: 1);

			BuildSpySatellite();
			_fog.Received().UpdateIsEnabled(numOfFriendlyStealthGenerators: 0, numOfEnemySpySatellites: 2);
        }

		[Test]
		public void RandomEnemyBuildingBuilt_DoesNotUpdate()
		{
            _enemyCruiser.CompleteConstructingBuliding(_randomBuilding);
            _fog.DidNotReceiveWithAnyArgs().UpdateIsEnabled(numOfFriendlyStealthGenerators: -99, numOfEnemySpySatellites: -99);
		}
		#endregion Enemy cruiser building completed

        [Test]
        public void EnemySpySatelliteDestroyed_Updates()
        {
            // Build satellite
            BuildSpySatellite();
            _fog.Received().UpdateIsEnabled(numOfFriendlyStealthGenerators: 0, numOfEnemySpySatellites: 1);

            // Destroy satellite
            DestroySpySatellite();
            _fog.Received().UpdateIsEnabled(numOfFriendlyStealthGenerators: 0, numOfEnemySpySatellites: 0);
        }

        [Test]
        public void Combo()
        {
            BuildStealthGenerator();
			_fog.Received().UpdateIsEnabled(numOfFriendlyStealthGenerators: 1, numOfEnemySpySatellites: 0);

            BuildSpySatellite();
			_fog.Received().UpdateIsEnabled(numOfFriendlyStealthGenerators: 1, numOfEnemySpySatellites: 1);

            BuildSpySatellite();
			_fog.Received().UpdateIsEnabled(numOfFriendlyStealthGenerators: 1, numOfEnemySpySatellites: 2);

            DestroyStealthGenerator();
			_fog.Received().UpdateIsEnabled(numOfFriendlyStealthGenerators: 0, numOfEnemySpySatellites: 2);

            DestroySpySatellite();
			_fog.Received().UpdateIsEnabled(numOfFriendlyStealthGenerators: 0, numOfEnemySpySatellites: 1);

            BuildStealthGenerator();
			_fog.Received().UpdateIsEnabled(numOfFriendlyStealthGenerators: 1, numOfEnemySpySatellites: 1);
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
