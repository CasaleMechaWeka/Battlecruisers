using System;
using UnityEngine;

namespace BattleCruisers.Tutorial.Highlighting
{
    public class HighlightFactory : MonoBehaviour, IHighlightFactory
    {
        public InGameHighlight inGameHighlightPrefab;
        public OnCanvasHighlight onCanvasHighlightPrefab;

        public IHighlight CreateHighlight(IHighlightable highlightable)
        {
            float radius = highlightable.Size.x / 2 * highlightable.SizeMultiplier;

            switch (highlightable.HighlightableType)
            {
                case HighlightableType.InGame:
                    Vector2 spawnPosition = (Vector2)highlightable.Transform.position + highlightable.PositionAdjustment;
                    return CreateInGameHighlight(radius, spawnPosition);

                case HighlightableType.OnCanvas:
                    return CreateOnCanvasHighlight(radius, highlightable.Transform, highlightable.PositionAdjustment);

                default:
                    throw new ArgumentException();
            }
        }

        // FELIX  Make private :)
        public IHighlight CreateInGameHighlight(float radiusInM, Vector2 position)
        {
            InGameHighlight inGameHighlight = Instantiate(inGameHighlightPrefab);
            inGameHighlight.Initialise(radiusInM, position);
            return inGameHighlight;
        }

        // FELIX  Make private :)
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
