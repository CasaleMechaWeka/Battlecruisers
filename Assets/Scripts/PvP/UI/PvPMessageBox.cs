using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using BattleCruisers.Data;
using BattleCruisers.UI.Sound.Players;
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
        public PvPCanvasGroupButton okBtn;
        private IDataProvider _dataProvider;
        private IPvPSingleSoundPlayer _soundPlayer;
        private Action _onClick;

        public void Initialize(IDataProvider dataProvider, IPvPSingleSoundPlayer soundPlayer, Action onClick = null)
        {
            _dataProvider = dataProvider;
            _soundPlayer = soundPlayer;
            _onClick = onClick;
            okBtn.Initialise(_soundPlayer, OnClick);
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
            panel.SetActive(true);
            blockUIPanel.SetActive(true);
        }

        private void OnClick()
        {
            _onClick?.Invoke();
            HideMessage();
        }

        public void HideMessage()
        {
            panel.SetActive(false);
            blockUIPanel.SetActive(false);
        }
    }
}
