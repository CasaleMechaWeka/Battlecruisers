using UnityEngine;

namespace BattleCruisers.Tutorial.Highlighting
{
    // FELIX  Remove (as well as test scene :) )
    public class HighlightFactory : MonoBehaviour, IHighlightFactory
    {
        public InGameHighlight inGameHighlightPrefab;
        public OnCanvasHighlight onCanvasHighlightPrefab;

        public IHighlight CreateInGameHighlight(float radiusInM, Vector2 position, bool usePulsingAnimation)
        {
            InGameHighlight inGameHighlight = Instantiate(inGameHighlightPrefab);
            inGameHighlight.Initialise(radiusInM, position, usePulsingAnimation);
            return inGameHighlight;
        }

        public IHighlight CreateOnCanvasHighlight(float radiusInPixels, Transform parentTransform, Vector2 positionAdjustment)
        {
            OnCanvasHighlight onCanvasHighlight = Instantiate(onCanvasHighlightPrefab, parentTransform);
            Vector3 currentPosition = onCanvasHighlight.transform.position;
            onCanvasHighlight.transform.position = new Vector3(currentPosition.x + positionAdjustment.x, currentPosition.y + positionAdjustment.y, currentPosition.z);
            onCanvasHighlight.Initialise(radiusInPixels);
            return onCanvasHighlight;
        }
    }
}
