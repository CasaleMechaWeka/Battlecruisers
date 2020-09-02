using System.Numerics;
using UnityEngine.Assertions;

namespace BattleCruisers.UI
{
    public enum TargetState
    {
        Shown, Hidden
    }

    // FELIX  Use, test
    public class SlidingPanel : Panel
    {
        private float _smoothTimeinS;
        private bool _haveReachedTarget;
        private TargetState _targetState;
        private TargetState TargetState
        {
            get => _targetState;
            set
            {
                _targetState = value;
                _haveReachedTarget = false;
                _smoothTimeinS = _targetState == TargetState.Shown ? showSmoothTimeInS : hideSmoothTimeInS;
            }
        }
        
        public Vector3 hiddenPosition, shownPosition;
        public float showSmoothTimeInS = 0.2f;
        public float hideSmoothTimeInS = 0.5f;

        void Start()
        {
            if (transform.position.Equals(hiddenPosition))
            {
                TargetState = TargetState.Hidden;
            }
            else if (transform.position.Equals(shownPosition))
            {
                TargetState = TargetState.Shown;
            }
            else
            {
                Assert.IsTrue(false, "Starting position should match shown or hidden position.");
            }
        }

        void Update()
        {
            // FELIX  NEXT  Adjust position, copying PatrollingMovementController :)
        }

        public override void Show()
        {
            if (TargetState == TargetState.Shown)
            {
                return;
            }

            gameObject.SetActive(true);
        }

        public override void Hide()
        {
            if (TargetState == TargetState.Hidden)
            {
                return;
            }

            gameObject.SetActive(false);
        }
    }
}