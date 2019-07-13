using System;
using UnityEngine;

namespace UnityCommon.Events
{
    public class Updater : MonoBehaviour, IUpdater
    {
        public event EventHandler Updated;

        void Update()
        {
            Updated?.Invoke(this, EventArgs.Empty);
        }
    }
}