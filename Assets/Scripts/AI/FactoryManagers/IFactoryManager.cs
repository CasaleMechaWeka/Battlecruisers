using BattleCruisers.Utils;

namespace BattleCruisers.AI.FactoryManagers
{
    /// <summary>
    /// Manages all the factories for a unit type.  Basically just decides which
    /// unit factories should build.
    /// </summary>
    public interface IFactoryManager : IManagedDisposable
    {
    }
}