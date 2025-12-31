using BattleCruisers.Utils;
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
            Assert.IsFalse(string.IsNullOrEmpty(name));
            Assert.IsFalse(string.IsNullOrEmpty(text));

            characterName.text = name;
            messageText.text = text;
        }
    }
}