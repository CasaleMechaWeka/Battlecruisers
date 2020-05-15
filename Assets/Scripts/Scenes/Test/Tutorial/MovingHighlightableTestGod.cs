using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Utils;
using BattleCruisers.Utils.DataStrctures;
using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.Scenes.Test.Tutorial
{
    public abstract class MovingHighlightableTestGod : MonoBehaviour
    {
        private ICoreHighlighter _highlighter;
        private IHighlightArgsFactory _highlightArgsFactory;
        private ICircularList<Button> _onCanvasButtons;
        private ICircularList<SpriteRenderer> _inGameObjects;

        private const int EXPECTED_NUM_OF_BUTTONS = 4;
        private const int EXPECTED_NUM_OF_IN_GAME_OBJECTS = 4;

        public Camera camera;
        public bool highlightGameObjects = true;

        void Start()
        {
            Assert.IsNotNull(camera);

            ICamera abstractCamera = new CameraBC(camera);
            _highlighter = CreateHighlighter(abstractCamera);
            _highlightArgsFactory = new HighlightArgsFactory(abstractCamera);

            Button[] onCanvasButtons = FindObjectsOfType<Button>();
            Assert.AreEqual(EXPECTED_NUM_OF_BUTTONS, onCanvasButtons.Length);
            _onCanvasButtons = new CircularList<Button>(onCanvasButtons);

            SpriteRenderer[] inGameObjects = FindObjectsOfType<SpriteRenderer>();
            Assert.AreEqual(EXPECTED_NUM_OF_IN_GAME_OBJECTS, inGameObjects.Length);
            _inGameObjects = new CircularList<SpriteRenderer>(inGameObjects);
            
            if (highlightGameObjects)
            {
                HighlightNextInGameObject();
            }
            else
            {
                HighlightNextButton();
            }
        }

        protected abstract ICoreHighlighter CreateHighlighter(ICamera camera);

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
            _highlighter.Highlight(highlightArgs);
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
            _highlighter.Highlight(highlightArgs);
        }
    }
}