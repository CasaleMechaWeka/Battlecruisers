using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using BattleCruisers.Scenes;

namespace BattleCruisers.UI.Common.BuildableDetails
{
    public class HeckleDetailsController : MonoBehaviour
    {
        public GameObject heckleMessage;
        public Text heckleText;


        public void ShowHeckle(string heckle)
        {
            
            heckleText.text = LandingSceneGod.Instance.hecklesStrings.GetString(heckle);
            heckleMessage.GetComponent<RectTransform>().localScale = Vector3.zero;
            heckleMessage.GetComponent<RectTransform>().DOScale(Vector3.one, 0.2f);
            gameObject.SetActive(true);
        }

        public void HideDetails()
        {
            gameObject.SetActive(false);
        }
    }
}
