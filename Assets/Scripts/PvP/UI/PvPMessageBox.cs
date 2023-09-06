using UnityEngine;
using UnityEngine.UI;
using BattleCruisers.Data;
using System;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Players;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI
{
    public class PvPMessageBox : MonoBehaviour
    {
        public static PvPMessageBox Instance;
        public GameObject panel;
        public GameObject blockUIPanel;
        public Text label;
        public Button okBtn;
        private Action _onClick;

        public void Initialize(Action onClick = null)
        {
            _onClick = onClick;
            okBtn.onClick.AddListener(() => _onClick?.Invoke());
            if (Instance == null)
                Instance = this;
        }

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
        }

        public void ShowMessage(string message, Action onClick = null)
        {
            label.text = message;
            _onClick = onClick;
            okBtn.onClick.AddListener(() => _onClick?.Invoke());
            panel.SetActive(true);
            blockUIPanel.SetActive(true);
        }
        public void HideMessage()
        {
            panel.SetActive(false);
            blockUIPanel.SetActive(false);
        }
    }
}
