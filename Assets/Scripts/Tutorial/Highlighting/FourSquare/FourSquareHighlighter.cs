using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Highlighting.FourSquare
{
    public class FourSquareHighlighter : MonoBehaviour, IMaskHighlighter
    {
        private IList<RectangleImage> _rectangles;
        private const int NUM_OF_RECTANGLES = 4;

        public void Initialise()
        {
            _rectangles = GetComponentsInChildren<RectangleImage>().ToList();
            Assert.AreEqual(NUM_OF_RECTANGLES, _rectangles.Count);

            foreach (RectangleImage rect in _rectangles)
            {
                rect.Initialise();
            }
        }

        public void Highlight(HighlightArgs args)
        {
            foreach (RectangleImage rect in _rectangles)
            {
                rect.UpdatePosition(args);
            }

            gameObject.SetActive(true);
        }

        public void Unhighlight()
        {
            gameObject.SetActive(false);
        }
    }
}