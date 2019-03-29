using System;
using UnityEngine;

namespace BattleCruisers.Utils.BattleScene.Update
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