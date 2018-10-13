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

        private const int EXPECTED_NUM_OF_BUTTONS = 4;

        public Camera camera;
        public GameObject inGameObject;
        public ConstDelayDeferrer deferrer;
        public MaskHighlighter maskHighlighter;

        void Start()
        {
            _highlightArgsFactory = new HighlightArgsFactory();

            deferrer.StaticInitialise(delayInMs: 2000);
            maskHighlighter.Initialise();

            // FELIX  Uncomment :P
            //Button[] onCanvasButtons = FindObjectsOfType<Button>();
            //Assert.AreEqual(EXPECTED_NUM_OF_BUTTONS, onCanvasButtons.Length);
            //_onCanvasButtons = new CircularList<Button>(onCanvasButtons);

            //HighlightNextButton();
            CreateInGameHighlight();
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

        private void CreateInGameHighlight()
        {
            SpriteRenderer renderer = inGameObject.GetComponent<SpriteRenderer>();
            Vector2 bottomLeftWorldPosition
                = new Vector2(
                    inGameObject.transform.position.x - renderer.size.x / 2,
                    inGameObject.transform.position.y - renderer.size.y / 2);
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
