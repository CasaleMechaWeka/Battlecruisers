using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BattleCruisers.UI.BattleScene.Navigation
{
    public class DragAndDropButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        private bool _isPointerDown;

        // FELIX  Replace with Initialise() :)
        private void Start()
        {
            _isPointerDown = false;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Debug.Log("OnPointerDown");
            _isPointerDown = true;

            // FELIX  Handle offset adjustment (ie, mouse position relative to our center position),
            // otherwise button "jumps" to mouse position :P
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            Debug.Log("OnPointerUp");
            _isPointerDown = false;
        }

        private void Update()
        {
            if (_isPointerDown)
            {
                transform.position = Input.mousePosition;
            }
        }
    }
}