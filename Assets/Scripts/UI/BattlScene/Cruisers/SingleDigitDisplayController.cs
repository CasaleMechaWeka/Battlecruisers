using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.BattleScene.Cruisers
{
    public class SingleDigitDisplayController : MonoBehaviour, INumberDisplay
    {
        private Text _text;

        private const int MIN_VALUE = 0;
        private const int MAX_VALUE = 9;

        public int Num
        {
            set
            {
                Assert.IsTrue(value >= MIN_VALUE);
                Assert.IsTrue(value <= MAX_VALUE);

                _text.text = value.ToString();
            }
        }

        public void Initialise()
        {
            _text = GetComponent<Text>();
            Assert.IsNotNull(_text);
        }
    }
}