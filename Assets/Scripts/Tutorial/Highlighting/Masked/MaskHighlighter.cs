using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Highlighting.Masked
{
    // FELIX  Remove
    public class MaskHighlighter : MonoBehaviour, IMaskHighlighter
    {
        private IList<MaskImage> _masks;
        private const int NUM_OF_MASKS = 4;

        public void Initialise()
        {
            _masks = GetComponentsInChildren<MaskImage>().ToList();
            Assert.AreEqual(NUM_OF_MASKS, _masks.Count);

            foreach (MaskImage mask in _masks)
            {
                mask.Initialise();
            }
        }

        public void Highlight(HighlightArgs args)
        {
            foreach (MaskImage mask in _masks)
            {
                mask.UpdatePosition(args);
            }

            gameObject.SetActive(true);
        }

        public void Unhighlight()
        {
            gameObject.SetActive(false);
        }
    }
}