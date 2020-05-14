using UnityEngine;
using System.Collections;
using BattleCruisers.Tutorial.Highlighting.Masked;
using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine.UI;
using UnityEngine.Assertions;
using BattleCruisers.Utils.DataStrctures;
using BattleCruisers.Utils;

namespace BattleCruisers.Scenes.Test.Tutorial
{
    public class MovingUIMaskTestGod : MonoBehaviour
    {
        private IHighlightArgsFactory _highlightArgsFactory;
        private ICircularList<Button> _onCanvasButtons;
        private ICircularList<SpriteRenderer> _inGameObjects;

        private const int EXPECTED_NUM_OF_BUTTONS = 4;
        private const int EXPECTED_NUM_OF_IN_GAME_OBJECTS = 4;

        public Camera camera;
        public InverseMaskHighlighter maskHighlighter;
        public bool highlightGameObjects = true;

        void Start()
        {
            Helper.AssertIsNotNull(camera, maskHighlighter);

            maskHighlighter.Initialise();

            _highlightArgsFactory = new HighlightArgsFactory(new CameraBC(camera));

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