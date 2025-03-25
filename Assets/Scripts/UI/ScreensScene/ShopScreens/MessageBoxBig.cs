using UnityEngine;
using UnityEngine.UI;
using BattleCruisers.Data;
using BattleCruisers.UI.Sound.Players;
using System;
using BattleCruisers.Scenes;
using BattleCruisers.Utils.Localisation;

namespace BattleCruisers.UI.ScreensScene.BattleHubScreen
{
    public class MessageBoxBig : MonoBehaviour
    {
        public GameObject panel;
        public Text title;
        public Text label;
        public Text closeButton;
        public CanvasGroupButton okBtn;
        private DataProvider _dataProvider;
        private ISingleSoundPlayer _soundPlayer;
        private Action _onClick;

        public void Initialize(DataProvider dataProvider, ISingleSoundPlayer soundPlayer, Action onClick = null)
        {
            _dataProvider = dataProvider;
            _soundPlayer = soundPlayer;
            _onClick = onClick;
            okBtn.Initialise(_soundPlayer, OnClick);
            panel.GetComponentInChildren<CanvasGroupButton>().Initialise(_soundPlayer, HideMessage);
        }

        private void Awake()
        {
        }

        public void ShowMessage(string titleText, string message, Action onClick = null, string closeButtonText = "")
        {
            if (closeButtonText == null || closeButtonText == "")
                closeButton.text = LocTableCache.ScreensSceneTable.GetString("UI/OkButton");
            else
                closeButton.text = closeButtonText;

            title.text = titleText;
            label.text = message;
            _onClick = onClick;
            panel.SetActive(true);
        }

        private void OnClick()
        {
            _onClick?.Invoke();
            HideMessage();
        }

        public void HideMessage()
        {
            panel.SetActive(false);
        }
    }
}
