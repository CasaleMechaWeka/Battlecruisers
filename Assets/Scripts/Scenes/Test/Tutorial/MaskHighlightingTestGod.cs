using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Tutorial.Highlighting.Masked;
using BattleCruisers.Utils;
using BattleCruisers.Utils.DataStrctures;
using BattleCruisers.Utils.PlatformAbstractions;
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
        public MaskHighlighter maskHighlighter;

        void Start()
        {
            _highlightArgsFactory = new HighlightArgsFactory(new CameraBC(camera));

            maskHighlighter.Initialise();

            Button[] onCanvasButtons = FindObjectsOfType<Button>();
            Assert.AreEqual(EXPECTED_NUM_OF_BUTTONS, onCanvasButtons.Length);
            _onCanvasButtons = new CircularList<Button>(onCanvasButtons);

            SpriteRenderer[] inGameObjects = FindObjectsOfType<SpriteRenderer>();
            Assert.AreEqual(EXPECTED_NUM_OF_IN_GAME_OBJECTS, inGameObjects.Length);
            _inGameObjects = new CircularList<SpriteRenderer>(inGameObjects);

            HighlightNextButton();
            //HighlightNextInGameObject();
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
            HighlightArgs highlightArgs = _highlightArgsFactory.CreateForOnCanvasObject(onCanvasObjRectTransform, sizeMultiplier: 1);
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
            HighlightArgs highlightArgs = _highlightArgsFactory.CreateForInGameObject(renderer.transform.position, renderer.size);
            maskHighlighter.Highlight(highlightArgs);
        }
    }
}
