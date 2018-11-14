using BattleCruisers.Targets.TargetTrackers;

namespace BattleCruisers.Cruisers
{
    public interface ICruiserFactoryNEW
    {
        Cruiser CreatePlayerCruiser();
        Cruiser CreateAICruiser();

        void InitialisePlayerCruiser(Cruiser playerCruiser, Cruiser aiCruiser);
        void InitialiseAICruiser(Cruiser playerCruiser, Cruiser aiCruiser, IUserChosenTargetHelper userChosenTargetHelper);
    }
}