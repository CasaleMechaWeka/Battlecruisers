namespace BattleCruisers.Utils
{
    // FELIX  Rename to DummyManagedDisposable, convention :P
    public class NullManagedDispoasable : IManagedDisposable
    {
        public void DisposeManagedState()
        {
            // empty
        }
    }
}
