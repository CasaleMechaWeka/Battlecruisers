namespace BattleCruisers.Buildables.Units
{
    /// <summary>
    /// Interface so completed building can be identified by casting, AND
    /// be mocked via NSubstitute.
    /// </summary>
    public interface ISpyPlaneController : IUnit { }
}
