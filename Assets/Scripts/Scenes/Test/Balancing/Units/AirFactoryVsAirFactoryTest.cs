using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets.Factories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using BCUtils = BattleCruisers.Utils;

namespace BattleCruisers.Scenes.Test.Balancing.Units
{
    public class AirFactoryVsAirFactoryTest : FactoryVsFactoryTest
    {
        protected override BuildableInitialisationArgs CreateFactoryArgs(Faction faction, Direction facingDirection)
        {
            IAircraftProvider aircraftProvider = CreateAircraftProvider(facingDirection);
            ITargetFactories targetFactories = CreateTargetFactories(facingDirection);

            return 
                new BuildableInitialisationArgs(
                    _helper, 
                    faction, 
                    parentCruiserDirection: facingDirection, 
                    aircraftProvider: aircraftProvider,
                    targetFactories: targetFactories);
        }

        private IAircraftProvider CreateAircraftProvider(Direction facingDirection)
        {
            BCUtils.IRandomGenerator random = new BCUtils.RandomGenerator();

            return
                IsLeftHandFactory(facingDirection) ?
                new AircraftProvider(parentCruiserPosition: _leftFactory.Position, enemyCruiserPosition: _rightFactory.Position, random: random) :
                new AircraftProvider(parentCruiserPosition: _rightFactory.Position, enemyCruiserPosition: _leftFactory.Position, random: random);
        }

        private ITargetFactories CreateTargetFactories(Direction facingDirection)
        {
            ObservableCollection<ITarget> targets = new ObservableCollection<ITarget>();

            // Defer adding target, so target has a chance to be initialised
            ITarget bomberTarget = IsLeftHandFactory(facingDirection) ? _rightFactory : _leftFactory;
            _deferrer.Defer(() => targets.Add(bomberTarget), delayInS: 0.1f);

            return _helper.CreateTargetFactories(targets);
        }

        private bool IsLeftHandFactory(Direction facingDirection)
        {
            switch (facingDirection)
            {
                case Direction.Left: 
                    return false;
                case Direction.Right:
                    return true;
                default:
                    throw new ArgumentException();
            }
        }
    }
}
