using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Highlighting.Arrows
{
    public class ArrowHighlighter : MonoBehaviour, ICoreHighlighter
    {
        private IArrowCalculator _calculator;

        public void Initialise(IArrowCalculator calculator)
        {
            Assert.IsNotNull(calculator);
            _calculator = calculator;
        }

        public void Highlight(HighlightArgs args)
        {
            if (!_calculator.ShouldShowArrow(args.Size))
            {
                return;
            }

            ArrowDirection arrowDirection = _calculator.FindArrowDirection(args.CenterPosition);
            Vector2 arrowHeadPosition = _calculator.FindArrowHeadPosition(args, arrowDirection);
            Vector2 upDirection = _calculator.FindArrowDirectionVector(arrowHeadPosition, args.CenterPosition);

            Logging.Log(Tags.MASKS, $"arrowDirection: {arrowDirection}  vector: {upDirection}  Head position: {arrowHeadPosition}");

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