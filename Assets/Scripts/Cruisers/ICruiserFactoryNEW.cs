using BattleCruisers.Targets.TargetTrackers;

namespace BattleCruisers.Cruisers
{
    public interface ICruiserFactoryNEW
    {
        ICruiser CreatePlayerCruiser();
        ICruiser CreateAICruiser();

        void InitialisePlayerCruiser(ICruiser playerCruiser, ICruiser aiCruiser);
        void InitialiseAICruiser(ICruiser playerCruiser, ICruiser aiCruiser, IUserChosenTargetHelper userChosenTargetHelper);
    }
}