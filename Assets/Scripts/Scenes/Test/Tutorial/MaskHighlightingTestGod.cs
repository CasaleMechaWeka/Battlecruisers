using BattleCruisers.Tutorial.Highlighting.Masked;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Threading;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Tutorial
{
    public class MaskHighlightingTestGod : MonoBehaviour
    {
        public GameObject inGameObject, onCanvasObject;
        public ConstDelayDeferrer deferrer;
        public MaskHighlighter maskHighlighter;

        void Start()
        {
            deferrer.StaticInitialise(delayInMs: 2000);
            maskHighlighter.Initialise();

            CreateOnCanvasHighglight();
            //CreateInGameHighlight();
        }

        private void CreateOnCanvasHighglight()
        {
            RectTransform onCanvasObjTransform = onCanvasObject.transform.Parse<RectTransform>();

            // FELIX  TEMP
            Vector3[] corners = new Vector3[4];
            onCanvasObjTransform.GetWorldCorners(corners);

            foreach (Vector3 corner in corners)
            {
                Debug.Log(corner);
            }


            Vector2 bottomLeftPosition
                = new Vector2(
                    onCanvasObjTransform.position.x - onCanvasObjTransform.rect.width / 2,
                    onCanvasObjTransform.position.y - onCanvasObjTransform.rect.height / 2);
            maskHighlighter.Highlight(bottomLeftPosition, onCanvasObjTransform.rect.size);
         
            //deferrer.Defer(maskHighlighter.Unhighlight);
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
