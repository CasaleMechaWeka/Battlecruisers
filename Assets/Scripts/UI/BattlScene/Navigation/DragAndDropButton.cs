using UnityEngine;
using UnityEngine.EventSystems;

namespace BattleCruisers.UI.BattleScene.Navigation
{
    public class DragAndDropButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        private bool _isPointerDown;
        // Offset adjustment (ie, mouse position relative to our center position),
        // to avoid button "jumping" to mouse position when first clicked :)
        private Vector3 _mouseToCenterOffset;

        // FELIX  Replace with Initialise() :)
        private void Start()
        {
            _isPointerDown = false;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Debug.Log("OnPointerDown");

            _isPointerDown = true;
            _mouseToCenterOffset = transform.position - Input.mousePosition;
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
                transform.position = Input.mousePosition + _mouseToCenterOffset;
            }
        }
    }
}