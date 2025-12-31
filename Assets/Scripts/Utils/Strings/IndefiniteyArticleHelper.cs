using System.Linq;
using UnityEngine.Localization.Settings;

namespace BattleCruisers.Utils.Strings
{
    public static class IndefiniteyArticleHelper
    {
        public static bool AddN(string noun)
        {
            bool isEnglish = LocalizationSettings.SelectedLocale.ToString().ToLower().Contains("english");

            return 
                isEnglish
                && StartsWithVowel(noun);
        }

        private static bool StartsWithVowel(string noun)
        {
            switch (noun.ToUpper().FirstOrDefault())
            {
                case 'A':
                case 'E':
                case 'I':
                case 'O':
                case 'U':
                    return true;
                default:
                    return false;
            }
        }
    }
}
