using System;
using UnityEngine.Assertions;

namespace BattleCruisers.UI
{
    public class ActionButton : CanvasGroupButton
    {
        private Action _onClick;

        public void Initialise(Action onClick)
        {
            base.Initialise();

            Assert.IsNotNull(onClick);
            _onClick = onClick;
        }

        protected override void OnClicked()
        {
            _onClick.Invoke();
        }
    }
}