using BattleCruisers.Tutorial.Highlighting.Masked;
using BattleCruisers.Utils;
using BattleCruisers.Utils.DataStrctures;
using BattleCruisers.Utils.Threading;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.Scenes.Test.Tutorial
{
    public class MaskHighlightingTestGod : MonoBehaviour
    {
        private IHighlightArgsFactory _highlightArgsFactory;
        private ICircularList<Button> _onCanvasButtons;
        private ICircularList<SpriteRenderer> _inGameObjects;

        private const int EXPECTED_NUM_OF_BUTTONS = 4;
        private const int EXPECTED_NUM_OF_IN_GAME_OBJECTS = 4;

        public Camera camera;
        public ConstDelayDeferrer deferrer;
        public MaskHighlighter maskHighlighter;

        void Start()
        {
            _highlightArgsFactory = new HighlightArgsFactory();

            deferrer.StaticInitialise(delayInMs: 2000);
            maskHighlighter.Initialise();

            Button[] onCanvasButtons = FindObjectsOfType<Button>();
            Assert.AreEqual(EXPECTED_NUM_OF_BUTTONS, onCanvasButtons.Length);
            _onCanvasButtons = new CircularList<Button>(onCanvasButtons);

            SpriteRenderer[] inGameObjects = FindObjectsOfType<SpriteRenderer>();
            Assert.AreEqual(EXPECTED_NUM_OF_IN_GAME_OBJECTS, inGameObjects.Length);
            _inGameObjects = new CircularList<SpriteRenderer>(inGameObjects);

            //HighlightNextButton();
            HighlightNextInGameObject();
        }

        private void HighlightNextButton()
        {
            Button buttonToHighlight = _onCanvasButtons.Next();
            CreateOnCanvasHighglight(buttonToHighlight.gameObject);

            Invoke("HighlightNextButton", time: 2);
        }

        private void CreateOnCanvasHighglight(GameObject onCanvasObject)
        {
            RectTransform onCanvasObjRectTransform = onCanvasObject.transform.Parse<RectTransform>();
            HighlightArgs highlightArgs = _highlightArgsFactory.CreateForOnCanvasObject(onCanvasObjRectTransform);
            maskHighlighter.Highlight(highlightArgs);
        }

        private void HighlightNextInGameObject()
        {
            SpriteRenderer objectToHighlight = _inGameObjects.Next();
            CreateInGameHighlight(objectToHighlight);

            Invoke("HighlightNextInGameObject", time: 2);
        }

        private void CreateInGameHighlight(SpriteRenderer renderer)
        {
            Vector2 bottomLeftWorldPosition
                = new Vector2(
                    renderer.transform.position.x - renderer.size.x / 2,
                    renderer.transform.position.y - renderer.size.y / 2);
            Vector2 bottomLeftScreenPosition = camera.WorldToScreenPoint(bottomLeftWorldPosition);

            float cameraHeight = 2 * camera.orthographicSize;
            float cameraWidth = camera.aspect * cameraHeight;

            float objectScreenWidthInPixels = renderer.size.x / cameraWidth * camera.pixelWidth;
            float objectScreenHeightInPixels = renderer.size.y / cameraHeight * camera.pixelHeight;
            Vector2 objectScreenSize = new Vector2(objectScreenWidthInPixels, objectScreenHeightInPixels);

            HighlightArgs highlightArgs = new HighlightArgs(bottomLeftScreenPosition, objectScreenSize);
            maskHighlighter.Highlight(highlightArgs);
        }
    }
}
