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

        public GameObject inGameObject;
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

            HighlightNextButton();
            //CreateInGameHighlight();
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

        //private void CreateInGameHighlight()
        //{
        //    SpriteRenderer renderer = inGameObject.GetComponent<SpriteRenderer>();
        //    float radius = renderer.size.x / 2;

        //    IHighlight inGameHighlight = factory.CreateInGameHighlight(radius, inGameObject.transform.position, usePulsingAnimation: true);

        //    deferrer.Defer(inGameHighlight.Destroy);
        //}
    }
}
