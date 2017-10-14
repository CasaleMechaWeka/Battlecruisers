namespace BattleCruisers.Utils
{
    /// <summary>
    /// Provides a common way for classes to clean themselves up (usually releasing
    /// event handlers).
    /// </summary>
    public interface IManagedDisposable
    {
        void DisposeManagedState();
    }
}
