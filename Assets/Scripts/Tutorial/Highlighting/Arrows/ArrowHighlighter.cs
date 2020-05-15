using BattleCruisers.Tutorial.Highlighting.Masked;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Highlighting.Arrows
{
    // FELIX  Only highlight small objects (eg:  Don't need to point arrow at cruiser :P)
    // FELIX  Don't make MonoBehaviour?
    // FELIX  Test
    public class ArrowHighlighter : MonoBehaviour, IMaskHighlighter
    {
        private IArrowCalculator _calculator;

        public void Initialise(IArrowCalculator calculator)
        {
            Assert.IsNotNull(calculator);
            _calculator = calculator;
        }

        public void Highlight(HighlightArgs args)
        {
            ArrowDirection arrowDirection = _calculator.FindArrowDirection(args.CenterPosition);
            Vector2 arrowHeadPosition = _calculator.FindArrowHeadPosition(args, arrowDirection);
            Vector2 upDirection = _calculator.FindArrowDirectionVector(arrowHeadPosition, args.CenterPosition);

            gameObject.transform.position = arrowHeadPosition;
            gameObject.transform.up = upDirection;

            gameObject.SetActive(true);
        }

        public void Unhighlight()
        {
            gameObject.SetActive(false);
        }
    }
}