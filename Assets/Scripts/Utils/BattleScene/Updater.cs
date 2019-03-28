using System;
using UnityEngine;

namespace BattleCruisers.Utils.BattleScene
{
    public class Updater : MonoBehaviour, IUpdater
    {
        public event EventHandler Update;

        void Start()
        {
            Update?.Invoke(this, EventArgs.Empty);
        }
    }
}