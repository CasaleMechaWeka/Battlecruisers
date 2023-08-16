using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using BattleCruisers.Data;
using BattleCruisers.UI.Sound.Players;

namespace BattleCruisers.UI.ScreensScene.BattleHubScreen
{
    public class MessageBox : MonoBehaviour
    {
        public static MessageBox Instance;
        public GameObject panel;
        public Text label;
        public CanvasGroupButton okBtn;
        private IDataProvider _dataProvider;
        private ISingleSoundPlayer _soundPlayer;
        public void Initialize(IDataProvider dataProvider, ISingleSoundPlayer soundPlayer)
        {
            _dataProvider = dataProvider;
            _soundPlayer = soundPlayer;
            okBtn.Initialise(_soundPlayer, OnClick);
            if (Instance == null)
                Instance = this;
        }

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
        }

        public void ShowMessage(string message)
        {
            label.text = message;
            panel.SetActive(true);
        }

        private void OnClick()
        {
            HideMessage();
        }

        public void HideMessage()
        {
            panel.SetActive(false);
        }
    }
}
