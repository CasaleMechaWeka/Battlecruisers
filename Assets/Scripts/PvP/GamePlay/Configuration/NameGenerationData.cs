using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Gameplay.Configuration
{
    public class NameGenerationData : ScriptableObject
    {
        [SerializeField]
        private string[] firstWordList = new string[] { "Happy", "Rich", "Brave", "Cool", "Calm" };
        [SerializeField]
        private string[] secondWordList = new string[] { "Hero", "Elf", "Dwarf", "Ranger", "Warden" };

        public string GenerateName()
        {
            string firstWord = firstWordList[Random.Range(0, firstWordList.Length)];
            string secondWord = secondWordList[Random.Range(0, secondWordList.Length)];

            return firstWord + " " + secondWord + Mathf.FloorToInt(Time.time).ToString();
        }
    }
}
