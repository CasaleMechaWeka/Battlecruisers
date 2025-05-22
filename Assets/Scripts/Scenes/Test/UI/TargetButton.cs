using BattleCruisers.UI.BattleScene;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace BattleCruisers.Scenes.Test.UI
{
    public class TargetButton : MonoBehaviour, IPointerClickHandler
    {
        private TargetIndicatorController _targetIndicator;

        public void Initialise(TargetIndicatorController targetIndicator)
        {
            Assert.IsNotNull(targetIndicator);
            _targetIndicator = targetIndicator;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _targetIndicator.Show(transform.position);
        }
    }
}