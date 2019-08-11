using System;
using UnityEngine;

namespace BattleCruisers.Utils.BattleScene.Update
{
    public class PhysicsUpdater : MonoBehaviour, IUpdater
    {
        public float DeltaTime => Time.deltaTime;

        public event EventHandler Updated;

        void FixedUpdate()
        {
            Updated?.Invoke(this, EventArgs.Empty);
        }
    }
}