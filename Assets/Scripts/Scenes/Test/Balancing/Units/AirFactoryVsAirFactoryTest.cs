using System;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Scenes.Test.Utilities;

namespace BattleCruisers.Scenes.Test.Balancing.Units
{
    public class AirFactoryVsAirFactoryTest : FactoryVsFactoryTest
    {
        protected override BuildableInitialisationArgs CreateFactoryArgs(Faction faction, Direction facingDirection)
        {
            IAircraftProvider aircraftProvider = CreateAircraftProvider(facingDirection);
            return 
                new BuildableInitialisationArgs(
                    _helper, 
                    faction, 
                    parentCruiserDirection: facingDirection, 
                    aircraftProvider: aircraftProvider);
        }

        private IAircraftProvider CreateAircraftProvider(Direction facingDirection)
        {
			if (facingDirection == Direction.Right)
			{
                // Left hand factory
                return new AircraftProvider(parentCruiserPosition: _leftFactory.Position, enemyCruiserPosition: _rightFactory.Position);
			}
			else if (facingDirection == Direction.Left)
			{
				// Right hand factory
                return new AircraftProvider(parentCruiserPosition: _rightFactory.Position, enemyCruiserPosition: _leftFactory.Position);
			}
			else
			{
				throw new ArgumentException();
			}
        }
    }
}
