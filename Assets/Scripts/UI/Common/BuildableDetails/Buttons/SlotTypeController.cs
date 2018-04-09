using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Utils.Fetchers;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.Common.BuildableDetails.Buttons
{
    public class SlotTypeController : MonoBehaviour
    {
        private ISpriteProvider _spriteProvider;
        private Image _slotImage;

        public SlotType SlotType
        {
            set
            {
                _slotImage.sprite = _spriteProvider.GetSlotSprite(value).Sprite; 
            }
        }

        public void Initialise(ISpriteProvider spriteProvider)
        {
            Assert.IsNotNull(spriteProvider);
            _spriteProvider = spriteProvider;

            _slotImage = GetComponent<Image>();
        }
    }
}
