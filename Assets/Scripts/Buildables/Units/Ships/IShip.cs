using BattleCruisers.Movement.Deciders;

namespace BattleCruisers.Buildables.Units.Ships
{
    public interface IShip : IUnit, IBasicMover 
    { 
        float OptimalArmamentRangeInM { get; }

        void DisableMovement();
    }
}
