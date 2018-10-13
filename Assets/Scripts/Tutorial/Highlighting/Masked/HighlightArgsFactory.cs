using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Tutorial.Highlighting.Masked
{
    public class HighlightArgsFactory : IHighlightArgsFactory
    {
        private readonly ICamera _camera;

        public HighlightArgsFactory(ICamera camera)
        {
            Assert.IsNotNull(camera);
            _camera = camera;
        }

        public HighlightArgs CreateForOnCanvasObject(RectTransform rectTransform)
        {
            Vector3[] corners = new Vector3[4];
            rectTransform.GetWorldCorners(corners);
            return new HighlightArgs(corners[0], rectTransform.sizeDelta);
        }

        public HighlightArgs CreateForInGameObject(Vector2 objectWorldPosition, Vector2 objectWorldSize)
        {
            return 
                new HighlightArgs(
                    FindBottomLeftScreenPosition(objectWorldPosition, objectWorldSize),
                    FindObjectScreenSize(objectWorldSize));
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
    }
}