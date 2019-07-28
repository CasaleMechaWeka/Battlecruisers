using System;
using UnityEngine;

namespace BattleCruisers.Utils.BattleScene.Update
{
    public class PhysicsUpdater : MonoBehaviour, IUpdater
    {
        public event EventHandler Updated;

        void FixedUpdate()
        {
            Updated?.Invoke(this, EventArgs.Empty);
        }
    }
}