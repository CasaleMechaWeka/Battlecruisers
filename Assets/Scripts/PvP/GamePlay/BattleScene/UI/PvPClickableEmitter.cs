using BattleCruisers.UI;
using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI
{
    public class PvPClickableEmitter : MonoBehaviour, IClickableEmitter, IPointerClickHandler
    {
        public event EventHandler Clicked;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (NetworkManager.Singleton != null)
                Clicked?.Invoke(this, EventArgs.Empty);
        }
    }
}