using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Highlighting.Masked
{
    public class HighlightArgsFactory : IHighlightArgsFactory
    {
        private readonly ICamera _camera;

        private const int HIGHLIGHT_BORDER_IN_PIXELS = 16;

        public HighlightArgsFactory(ICamera camera)
        {
            Assert.IsNotNull(camera);
            _camera = camera;
        }

        public HighlightArgs CreateForOnCanvasObject(RectTransform rectTransform, float sizeMultiplier)
        {
            Vector2 bottomLeftPosition = FindBottomLeftPosition(rectTransform, sizeMultiplier);
            Vector2 size = sizeMultiplier * rectTransform.sizeDelta * rectTransform.lossyScale;
            HighlightArgs noBorderArgs = new HighlightArgs(bottomLeftPosition, size);
            return AddBorder(noBorderArgs);
        }

        private Vector2 FindBottomLeftPosition(RectTransform rectTransform, float sizeMultiplier)
        {
            float xAdjustment = rectTransform.sizeDelta.x * (1 - sizeMultiplier) / 2;
            float yAdjustment = rectTransform.sizeDelta.y * (1 - sizeMultiplier) / 2;

            Vector3[] corners = new Vector3[4];
            rectTransform.GetWorldCorners(corners);
            Vector2 bottomLeftCorner = corners[0];

            return new Vector2(bottomLeftCorner.x + xAdjustment, bottomLeftCorner.y + yAdjustment);
        }

        public HighlightArgs CreateForInGameObject(Vector2 objectWorldPosition, Vector2 objectWorldSize)
        {
            HighlightArgs noBorderArgs 
                = new HighlightArgs(
                    FindBottomLeftScreenPosition(objectWorldPosition, objectWorldSize),
                    FindObjectScreenSize(objectWorldSize));
            return AddBorder(noBorderArgs);
        }

        private Vector2 FindBottomLeftScreenPosition(Vector2 objectWorldPosition, Vector2 objectWorldSize)
        {
            Vector2 bottomLeftWorldPosition
                = new Vector2(
                    objectWorldPosition.x - objectWorldSize.x / 2,
                    objectWorldPosition.y - objectWorldSize.y / 2);
            return _camera.WorldToScreenPoint(bottomLeftWorldPosition);
        }

        private Vector2 FindObjectScreenSize(Vector2 objectWorldSize)
        {
            Vector2 cameraSize = _camera.GetSize();
            float objectScreenWidthInPixels = objectWorldSize.x / cameraSize.x * _camera.PixelWidth;
            float objectScreenHeightInPixels = objectWorldSize.y / cameraSize.y * _camera.PixelHeight;
            return new Vector2(objectScreenWidthInPixels, objectScreenHeightInPixels);
        }

        private HighlightArgs AddBorder(HighlightArgs noBorderArgs)
        {
            return
                new HighlightArgs(
                    new Vector2(noBorderArgs.BottomLeftPosition.x - HIGHLIGHT_BORDER_IN_PIXELS, noBorderArgs.BottomLeftPosition.y - HIGHLIGHT_BORDER_IN_PIXELS),
                    new Vector2(noBorderArgs.Size.x + 2 * HIGHLIGHT_BORDER_IN_PIXELS, noBorderArgs.Size.y + 2 * HIGHLIGHT_BORDER_IN_PIXELS));
        }
    }
}