using UnityEngine;
using UnityEngine.UI;
using BattleCruisers.Data;
using BattleCruisers.UI.Sound.Players;
using System;
using BattleCruisers.Scenes;
using BattleCruisers.Utils.Localisation;

namespace BattleCruisers.UI.ScreensScene.BattleHubScreen
{
    public class MessageBox : MonoBehaviour
    {
        public GameObject panel;
        public Text label;
        public Text closeButton;
        public CanvasGroupButton okBtn;
        private IDataProvider _dataProvider;
        private ISingleSoundPlayer _soundPlayer;
        private Action _onClick;

        public void Initialize(IDataProvider dataProvider, ISingleSoundPlayer soundPlayer, Action onClick = null)
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

        public void ShowMessage(string message, Action onClick = null, string closeButtonText = "")
        {
            if (closeButtonText == null || closeButtonText == "")
            {
                if (closeButton != null)
                    closeButton.text = LocTableFactory.ScreensSceneTable.GetString("UI/OkButton");
            }
            else
            {
                if (closeButton != null)
                    closeButton.text = closeButtonText;
            }

            label.text = message;
            _onClick = onClick;
            panel.SetActive(true);
            Invoke("HideMessage", 3f);
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
