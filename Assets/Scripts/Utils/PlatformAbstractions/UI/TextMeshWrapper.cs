using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils.PlatformAbstractions.UI
{
    public class TextMeshWrapper : ITextMesh
    {
        private readonly TextMesh _textMesh;

        public string Text
        {
            get { return _textMesh.text; }
            set { _textMesh.text = value; }
        }

        public TextMeshWrapper(TextMesh textMesh)
        {
            Assert.IsNotNull(textMesh);
            _textMesh = textMesh;
        }

        public void SetActive(bool isActive)
        {
            _textMesh.gameObject.SetActive(isActive);
        }
    }
}
