using UnityEngine;

namespace BattleCruisers.Tutorial.Highlighting
{
    public class HighlightFactory : MonoBehaviour, IHighlightFactory
    {
        public InGameHighlight inGameHighlightPrefab;
        public OnCanvasHighlight onCanvasHighlightPrefab;

        public IHighlight CreateInGameHighlight(float radiusInM, Vector2 position)
        {
            InGameHighlight inGameHighlight = Instantiate(inGameHighlightPrefab);
            inGameHighlight.Initialise(radiusInM, position);
            return inGameHighlight;
        }

        public IHighlight CreateOnCanvasHighlight(float radiusInPixels, Transform parentTransform)
        {
            OnCanvasHighlight onCanvasHighlight = Instantiate(onCanvasHighlightPrefab, parentTransform);
            onCanvasHighlight.Initialise(radiusInPixels);
            return onCanvasHighlight;
        }
    }
}
