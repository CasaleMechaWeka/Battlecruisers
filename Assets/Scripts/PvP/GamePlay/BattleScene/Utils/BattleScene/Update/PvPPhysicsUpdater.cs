using System;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Update
{
    public class PvPPhysicsUpdater : MonoBehaviour, IPvPUpdater
    {
        public float DeltaTime => Time.deltaTime;

        public event EventHandler Updated;

        void FixedUpdate()
        {
            Updated?.Invoke(this, EventArgs.Empty);
        }
    }
}