using BattleCruisers.Utils;
using System;
using UnityEngine;

// FELIX  Make testable?  Use:  IInput & IUpdater
// FELIX  Add tutorial step :)
namespace BattleCruisers.UI.Cameras.Helpers.Pinch
{
    public class PinchTracker : MonoBehaviour, IPinchTracker
    {
        private float _lastDistanceInM;

        private bool _isPinching;
        private bool IsPinching
        {
            get => _isPinching;
            set
            {
                if (_isPinching == value)
                {
                    return;
                }

                _isPinching = value;

                if (_isPinching)
                {
                    Logging.Log(Tags.PINCH, $"About to invoke {nameof(PinchStart)} event :D");
                    PinchStart.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    Logging.Log(Tags.PINCH, $"About to invoke {nameof(PinchEnd)} event :P");
                    PinchEnd.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public event EventHandler PinchStart;
        public event EventHandler<PinchEventArgs> Pinch;
        public event EventHandler PinchEnd;

        void Update()
        {
            if (Input.touchCount == 2)
            {
                if (!IsPinching)
                {
                    IsPinching = true;
                    _lastDistanceInM = Vector2.Distance(Input.touches[0].position, Input.touches[1].position);
                }
                else
                {
                    OnPinch();
                }
            }
            else
            {
                IsPinching = false;
            }
        }

        void OnPinch()
        {
            Vector2 touchPosition1 = Input.touches[0].position;
            Vector2 touchPosition2 = Input.touches[1].position;
            
            float currentDistance = Vector2.Distance(touchPosition1, touchPosition2);
            float delta = currentDistance - _lastDistanceInM;
            _lastDistanceInM = currentDistance;

            Logging.Log(Tags.PINCH, $"About to invoke {nameof(Pinch)} event with position: {touchPosition1}  and delta: {delta}");
            Pinch.Invoke(this, new PinchEventArgs(touchPosition1, delta));
        }
    }
}