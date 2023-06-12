using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace BattleCruisers.Network.Multiplay.Gameplay.UI
{/// <summary>
 /// Simple popup panel to display information to players.
 /// </summary>
    public class PopupPanel : MonoBehaviour
    {
        [SerializeField]
        TextMeshProUGUI m_TitleText;
        [SerializeField]
        TextMeshProUGUI m_MainText;
        [SerializeField]
        GameObject m_ConfirmButton;
        [SerializeField]
        GameObject m_LoadingSpinner;
        [SerializeField]
        CanvasGroup m_CanvasGroup;

        public bool IsDisplaying => m_IsDisplaying;

        bool m_IsDisplaying;

        bool m_ClosableByUser;

        void Awake()
        {
            Hide();
        }

        public void OnConfirmClick()
        {
            if (m_ClosableByUser)
            {
                Hide();
            }
        }

        public void SetupPopupPanel(string titleText, string mainText, bool closeableByUser = true, UnityAction _onClick = null)
        {
            m_TitleText.text = titleText;
            m_MainText.text = mainText;
            m_ClosableByUser = closeableByUser;
            m_ConfirmButton.SetActive(m_ClosableByUser);
            if(m_ClosableByUser)
            {
                m_ConfirmButton.GetComponent<Button>().onClick.AddListener(_onClick);
                m_ConfirmButton.GetComponent<Button>().onClick.AddListener(OnConfirmClick);
            }
         
            m_LoadingSpinner.SetActive(!m_ClosableByUser);
            Show();
        }

        void Show()
        {
            m_CanvasGroup.alpha = 1f;
            m_CanvasGroup.blocksRaycasts = true;
            m_IsDisplaying = true;
        }

        public void Hide()
        {
            m_CanvasGroup.alpha = 0f;
            m_CanvasGroup.blocksRaycasts = false;
            m_IsDisplaying = false;
            m_ConfirmButton.GetComponent<Button>().onClick.RemoveAllListeners();
        }
    }

}


