using System;

namespace UnityCommon.Events
{
    public interface IUpdater
    {
        // Called once per frame
        event EventHandler Updated;
    }
}