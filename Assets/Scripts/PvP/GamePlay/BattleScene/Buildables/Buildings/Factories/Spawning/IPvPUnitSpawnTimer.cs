namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Factories.Spawning
{
    public interface IPvPUnitSpawnTimer
    {
        /// <summary>
        /// The time in seconds since the factory last completed a unit,
        /// or an in progress unit was destroyed.
        /// </summary>
        float TimeSinceFactoryWasClearInS { get; }

        float TimeSinceUnitWasChosenInS { get; }
    }
}