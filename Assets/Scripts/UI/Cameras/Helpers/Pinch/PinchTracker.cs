using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene.Update;
using BattleCruisers.Utils.PlatformAbstractions;
using System;
using UnityEngine;

namespace BattleCruisers.UI.Cameras.Helpers.Pinch
{
    public class PinchTracker : IPinchTracker
    {
        private readonly IInput _input;
        private readonly IUpdater _updater;

        private float _lastDistanceInM;

        public PinchTracker(IInput input, IUpdater updater)
        {
            Helper.AssertIsNotNull(input, updater);

            _input = input;
            _updater = updater;
            _isPinching = false;

            _updater.Updated += _updater_Updated;
        }

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

        private void _updater_Updated(object sender, EventArgs e)
        {
            if (_input.TouchCount == 2)
            {
                if (!IsPinching)
                {
                    IsPinching = true;
                    _lastDistanceInM = Vector2.Distance(_input.GetTouchPosition(0), _input.GetTouchPosition(1));
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
            Vector2 touchPosition1 = _input.GetTouchPosition(0);
            Vector2 touchPosition2 = _input.GetTouchPosition(1);
            
            float currentDistance = Vector2.Distance(touchPosition1, touchPosition2);
            float delta = currentDistance - _lastDistanceInM;
            _lastDistanceInM = currentDistance;

            Logging.Log(Tags.PINCH, $"About to invoke {nameof(Pinch)} event with position: {touchPosition1}  and delta: {delta}");
            Pinch.Invoke(this, new PinchEventArgs(touchPosition1, delta));
        }
    }
}