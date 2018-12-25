using System;

namespace BattleCruisers.Buildables
{
    // FELIX   Move to right folder
    public class DestroyedEventArgs : EventArgs
    {
        public ITarget DestroyedTarget { get; private set; }

        public DestroyedEventArgs(ITarget destroyedTarget)
        {
            DestroyedTarget = destroyedTarget;
        }
    }

    public interface IDestructable
    {
        event EventHandler<DestroyedEventArgs> Destroyed;

        void Destroy();
    }
}