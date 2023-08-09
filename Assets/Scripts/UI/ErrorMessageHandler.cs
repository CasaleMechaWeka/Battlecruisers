using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Threading.Tasks;

namespace BattleCruisers.UI
{
    public class ErrorMessageHandler : MonoBehaviour
    {
        private Image messageBox;
        private Text message;
        private bool isShowing;
        private void Start()
        {
            messageBox = GetComponent<Image>();
            message = GetComponentInChildren<Text>();
            messageBox.enabled = false;
            message.text = string.Empty;
            transform.localScale = Vector3.zero;
            DOTween.Init();
        }

        public async void ShowMessage(string msg)
        {
            if(!isShowing)
            {
                isShowing = true;
                message.text = msg;
                messageBox.enabled = true;
                transform.DOScale(new Vector3(1f, 1f, 0), 0.3f);
                await Task.Delay(3000);
                transform.DOScale(Vector3.zero, 0.3f);
                isShowing = false;
            }
        }
    }
}

