using BattleCruisers.Utils;
using Castle.Core.Internal;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.TrashScreen
{
    public class BubbleController : MonoBehaviour
    {
        public Text characterName, messageText;

        public void Initialise(string name, string text)
        {
            Helper.AssertIsNotNull(characterName, messageText);
            Assert.IsFalse(name.IsNullOrEmpty());
            Assert.IsFalse(text.IsNullOrEmpty());

            characterName.text = name;
            messageText.text = text;
        }
    }
}